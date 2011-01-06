using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.Common.Extensions;
using ServiceStack.Common.Utils;
using ServiceStack.Redis;

namespace ServiceStack.Questions.ServiceInterface
{
	public class Repository : IRepository
	{
		private const string QuestionUpVotesPrefix = "urn:upvotes:question:";
		private const string QuestionDownVotesPrefix = "urn:downvotes:question:";
		private const string AnswerUpVotesPrefix = "urn:upvotes:answer:";
		private const string AnswerDownVotesPrefix = "urn:downvotes:answer:";

		IRedisClientsManager RedisManager { get; set; }

		public Repository(IRedisClientsManager redisManager)
		{
			RedisManager = redisManager;
		}

		public User GetOrCreateUser(User user)
		{
			if (user.DisplayName.IsNullOrEmpty())
				throw new ArgumentNullException("DisplayName");

			var userIdAliasKey = "id:User:DisplayName:" + user.DisplayName.ToLower();

			using (var redis = RedisManager.GetClient())
			using (var redisUsers = redis.GetTypedClient<User>())
			{
				//Find user by DisplayName
				var userKey = redis.GetValue(userIdAliasKey);
				if (userKey != null)
					return redisUsers.GetValue(userKey);

				//Generate Id for New User
				if (user.Id == default(long))
					user.Id = redisUsers.GetNextSequence();

				//Create or Update New User
				redisUsers.Store(user);
				//Save reference to User key using the DisplayName alias
				redis.SetEntry(userIdAliasKey, user.CreateUrn());

				//Retrieve the User by Id
				return redisUsers.GetById(user.Id);
			}
		}

		public List<Question> GetAllQuestions()
		{
			return RedisManager.Exec<Question>(redisQuestions => redisQuestions.GetAll()).ToList();
		}

		public List<QuestionResult> GetRecentQuestionResults(int skip, int take)
		{
			using (var redis = RedisManager.GetReadOnlyClient())
			using (var redisQuestions = redis.GetTypedClient<Question>())
			{
				var recentQuestions = redisQuestions.GetLatestFromRecentsList(skip, take);
				var uniqueUserIds = recentQuestions.ConvertAll(x => x.UserId).ToHashSet();
				var usersMap = GetUsersByIds(uniqueUserIds).ToDictionary(x => x.Id);

				var results = recentQuestions.ConvertAll(x => new QuestionResult { Question = x });
				var resultsMap = results.ToDictionary(q => q.Question.Id);

				results.ForEach(x => x.User = usersMap[x.Question.UserId]);

				//Batch multiple operations in a single transaction which is pipelined (i.e. processed in a single request/response)
				using (var trans = redis.CreateTransaction())
				{
					foreach (var question in recentQuestions)
					{
						var q = question;
						trans.QueueCommand(r => r.GetSetCount(QuestionUpVotesPrefix + q.Id), voteUpCount => resultsMap[q.Id].VotesUpCount = voteUpCount);
						trans.QueueCommand(r => r.GetSetCount(QuestionUpVotesPrefix + q.Id), voteDownCount => resultsMap[q.Id].VotesDownCount = voteDownCount);
						trans.QueueCommand(r => r.GetTypedClient<Question>().GetRelatedEntitiesCount<Answer>(q.Id), answersCount => resultsMap[q.Id].AnswersCount = answersCount);
					}

					trans.Commit();
				}

				return results;
			}
		}

		public void StoreQuestion(Question question)
		{
			RedisManager.Exec<Question>(redisQuestions =>
			{
				if (question.Id == default(long))
				{
					question.Id = redisQuestions.GetNextSequence();
					question.CreatedDate = DateTime.UtcNow;
				}

				redisQuestions.Store(question);
				redisQuestions.AddToRecentsList(question);
			});
		}

		public void StoreAnswer(Answer answer)
		{
			using (var redis = RedisManager.GetClient())
			using (var redisQuestions = redis.GetTypedClient<Question>())
			using (var redisAnswers = redis.GetTypedClient<Answer>())
			{
				if (answer.Id == default(long))
					answer.Id = redisAnswers.GetNextSequence();

				redisAnswers.Store(answer);
				redisQuestions.StoreRelatedEntities(answer.QuestionId, answer);
			}
		}

		public List<Answer> GetAnswersForQuestion(long questionId)
		{
			using (var redis = RedisManager.GetClient())
			using (var redisQuestions = redis.GetTypedClient<Question>())
			{
				return redisQuestions.GetRelatedEntities<Answer>(questionId);
			}
		}

		public void VoteQuestionUp(long userId, long questionId)
		{
			using (var redis = RedisManager.GetClient())
			{
				redis.AddItemToSet(QuestionUpVotesPrefix + questionId, userId.ToString());
				redis.RemoveItemFromSet(QuestionDownVotesPrefix + questionId, userId.ToString());
			}
		}

		public void VoteQuestionDown(long userId, long questionId)
		{
			using (var redis = RedisManager.GetClient())
			{
				redis.AddItemToSet(QuestionDownVotesPrefix + questionId, userId.ToString());
				redis.RemoveItemFromSet(QuestionUpVotesPrefix + questionId, userId.ToString());
			}
		}

		public void VoteAnswerUp(long userId, long answerId)
		{
			using (var redis = RedisManager.GetClient())
			{
				redis.AddItemToSet(AnswerUpVotesPrefix + answerId, userId.ToString());
				redis.RemoveItemFromSet(AnswerDownVotesPrefix + answerId, userId.ToString());
			}
		}

		public void VoteAnswerDown(long userId, long answerId)
		{
			using (var redis = RedisManager.GetClient())
			{
				redis.AddItemToSet(AnswerDownVotesPrefix + answerId, userId.ToString());
				redis.RemoveItemFromSet(AnswerUpVotesPrefix + answerId, userId.ToString());
			}
		}

		public Question GetQuestion(long questionId)
		{
			return RedisManager.Exec<Question>(redisQuestions => redisQuestions.GetById(questionId));
		}

		public List<User> GetUsersByIds(IEnumerable<long> userIds)
		{
			return RedisManager.Exec<User>(redisUsers => redisUsers.GetByIds(userIds)).ToList();
		}

		public QuestionStat GetQuestionStats(long questionId)
		{
			using (var redis = RedisManager.GetReadOnlyClient())
			{
				var result = new QuestionStat
				{
					VotesUpCount = redis.GetSetCount(QuestionUpVotesPrefix + questionId),
					VotesDownCount = redis.GetSetCount(QuestionDownVotesPrefix + questionId)
				};
				result.VotesTotal = result.VotesUpCount - result.VotesDownCount;
				return result;
			}
		}
	}
}