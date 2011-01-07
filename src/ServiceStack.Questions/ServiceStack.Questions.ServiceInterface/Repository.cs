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
		static class QuestionUserIndex
		{
			public static string UpVotes(long questionId) { return "set:q>user+:" + questionId; }
			public static string DownVotes(long questionId) { return "set:q>user-:" + questionId; }
		}

		static class UserQuestionIndex
		{
			public static string Questions(long userId) { return "set:user>q:" + userId; }
			public static string UpVotes(long userId) { return "set:user>q+:" + userId; }
			public static string DownVotes(long userId) { return "set:user>q-:" + userId; }
		}

		static class AnswerUserIndex
		{
			public static string UpVotes(long answerId) { return "set:a>user+:" + answerId; }
			public static string DownVotes(long answerId) { return "set:a>user-:" + answerId; }
		}

		static class UserAnswerIndex
		{
			public static string Answers(long userId) { return "set:user>a:" + userId; }
			public static string UpVotes(long userId) { return "set:user>a+:" + userId; }
			public static string DownVotes(long userId) { return "set:user>a-:" + userId; }
		}

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

		public UserStat GetUserStats(long userId)
		{
			using (var redis = RedisManager.GetClient())
			{
				return new UserStat
				{
                    UserId = userId,
					QuestionsCount = redis.GetSetCount(UserQuestionIndex.Questions(userId)),
					AnswersCount = redis.GetSetCount(UserAnswerIndex.Answers(userId)),
				};
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
						trans.QueueCommand(r => r.GetSetCount(QuestionUserIndex.UpVotes(q.Id)), voteUpCount => resultsMap[q.Id].VotesUpCount = voteUpCount);
						trans.QueueCommand(r => r.GetSetCount(QuestionUserIndex.DownVotes(q.Id)), voteDownCount => resultsMap[q.Id].VotesDownCount = voteDownCount);
						trans.QueueCommand(r => r.GetTypedClient<Question>().GetRelatedEntitiesCount<Answer>(q.Id), answersCount => resultsMap[q.Id].AnswersCount = answersCount);
					}

					trans.Commit();
				}

				return results;
			}
		}

		public void StoreQuestion(Question question)
		{
			using (var redis = RedisManager.GetClient())
			using (var redisQuestions = redis.GetTypedClient<Question>())
			{
				if (question.Id == default(long))
				{
					question.Id = redisQuestions.GetNextSequence();
					question.CreatedDate = DateTime.UtcNow;
				}

				redisQuestions.Store(question);
				redisQuestions.AddToRecentsList(question);				
				redis.AddItemToSet(UserQuestionIndex.Questions(question.UserId), question.Id.ToString());
			}
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
				redis.AddItemToSet(UserAnswerIndex.Answers(answer.UserId), answer.Id.ToString());
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
			//Populate Question => User and User => Question set indexes in a single transaction
			RedisManager.ExecTrans(trans =>
			{
				//Register upvote against question and remove any downvotes if any
				trans.QueueCommand(redis => redis.AddItemToSet(QuestionUserIndex.UpVotes(questionId), userId.ToString()));
				trans.QueueCommand(redis => redis.RemoveItemFromSet(QuestionUserIndex.DownVotes(questionId), userId.ToString()));

				//Register upvote against user and remove any downvotes if any
				trans.QueueCommand(redis => redis.AddItemToSet(UserQuestionIndex.UpVotes(userId), questionId.ToString()));
				trans.QueueCommand(redis => redis.RemoveItemFromSet(UserQuestionIndex.DownVotes(userId), questionId.ToString()));
			});
		}

		public void VoteQuestionDown(long userId, long questionId)
		{
			//Populate Question => User and User => Question set indexes in a single transaction
			RedisManager.ExecTrans(trans =>
			{
				//Register downvote against question and remove any upvotes if any
				trans.QueueCommand(redis => redis.AddItemToSet(QuestionUserIndex.DownVotes(questionId), userId.ToString()));
				trans.QueueCommand(redis => redis.RemoveItemFromSet(QuestionUserIndex.UpVotes(questionId), userId.ToString()));

				//Register downvote against user and remove any upvotes if any
				trans.QueueCommand(redis => redis.AddItemToSet(UserQuestionIndex.DownVotes(userId), questionId.ToString()));
				trans.QueueCommand(redis => redis.RemoveItemFromSet(UserQuestionIndex.UpVotes(userId), questionId.ToString()));
			});
		}

		public void VoteAnswerUp(long userId, long answerId)
		{
			//Populate Question => User and User => Question set indexes in a single transaction
			RedisManager.ExecTrans(trans =>
			{
				//Register upvote against answer and remove any downvotes if any
				trans.QueueCommand(redis => redis.AddItemToSet(AnswerUserIndex.UpVotes(answerId), userId.ToString()));
				trans.QueueCommand(redis => redis.RemoveItemFromSet(AnswerUserIndex.DownVotes(answerId), userId.ToString()));

				//Register upvote against user and remove any downvotes if any
				trans.QueueCommand(redis => redis.AddItemToSet(UserAnswerIndex.UpVotes(userId), answerId.ToString()));
				trans.QueueCommand(redis => redis.RemoveItemFromSet(UserAnswerIndex.DownVotes(userId), answerId.ToString()));
			});
		}

		public void VoteAnswerDown(long userId, long answerId)
		{
			//Populate Question => User and User => Question set indexes in a single transaction
			RedisManager.ExecTrans(trans =>
			{
				//Register downvote against answer and remove any upvotes if any
				trans.QueueCommand(redis => redis.AddItemToSet(AnswerUserIndex.DownVotes(answerId), userId.ToString()));
				trans.QueueCommand(redis => redis.RemoveItemFromSet(AnswerUserIndex.UpVotes(answerId), userId.ToString()));

				//Register downvote against user and remove any upvotes if any
				trans.QueueCommand(redis => redis.AddItemToSet(AnswerUserIndex.DownVotes(userId), answerId.ToString()));
				trans.QueueCommand(redis => redis.RemoveItemFromSet(AnswerUserIndex.UpVotes(userId), answerId.ToString()));
			});
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
					VotesUpCount = redis.GetSetCount(QuestionUserIndex.UpVotes(questionId)),
					VotesDownCount = redis.GetSetCount(QuestionUserIndex.DownVotes(questionId))
				};
				result.VotesTotal = result.VotesUpCount - result.VotesDownCount;
				return result;
			}
		}

		public SiteStats GetSiteStats()
		{
			using (var redis = RedisManager.GetClient())
			using (var redisQuestions = redis.GetTypedClient<Question>())
			using (var redisAnswers = redis.GetTypedClient<Answer>())
			{
				return new SiteStats
				{
					QuestionsCount = redisQuestions.TypeIdsSet.Count,
                    AnswersCount = redisAnswers.TypeIdsSet.Count
				};
			}
		}
	}
}