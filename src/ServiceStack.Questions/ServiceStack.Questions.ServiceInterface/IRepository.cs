using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.Common.Extensions;
using ServiceStack.Common.Utils;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace ServiceStack.Questions.ServiceInterface
{
	public interface IRepository
	{
		User GetOrCreateUser(User user);

		List<Question> GetAllQuestions();

		List<Question> GetRecentQuestions(int skip, int take);

		void StoreQuestion(Question question);

		void StoreAnswer(Answer answer);

		List<Answer> GetAnswersForQuestion(long questionId);

		void VoteQuestionUp(long userId, long questionId);

		void VoteQuestionDown(long userId, long questionId);

		QuestionStat GetQuestionStats(long questionId);
	}

	public class Repository : IRepository
	{
		private const string UpVotesPrefix = "urn:upvotes:question:";
		private const string DownVotesPrefix = "urn:downvotes:question:";

		IRedisClientsManager RedisManager { get; set; }

		public Repository(IRedisClientsManager redisManager)
		{
			RedisManager = redisManager;
		}

		public User GetOrCreateUser(User user)
		{
			if (user.DisplayName.IsNullOrEmpty())
				throw new ArgumentNullException("user.DisplayName");

			var userIdNameMapKey = "id:User:DisplayName:" + user.DisplayName.ToLower();

			using (var redis = RedisManager.GetClient())
			using (var redisUsers = redis.GetTypedClient<User>())
			{
				var userKey = redis.GetValue(userIdNameMapKey);
				if (userKey != null)
					return redisUsers.GetValue(userKey);

				if (user.Id != default(long))
					user.Id = redisUsers.GetNextSequence();

				redisUsers.Store(user);
				redis.SetEntry(userIdNameMapKey, user.CreateUrn());

				return redisUsers.GetById(user.Id);
			}
		}

		//Reduce the boilerplate for common access
		T ExecRedisQuestions<T>(Func<IRedisTypedClient<Question>, T> lamda)
		{
			using (var redis = RedisManager.GetClient())
			using (var redisQuestions = redis.GetTypedClient<Question>())
			{
				return lamda(redisQuestions);
			}
		}

		public List<Question> GetAllQuestions()
		{
			return ExecRedisQuestions(q => q.GetAll().ToList());
		}

		public List<Question> GetRecentQuestions(int skip, int take)
		{
			return ExecRedisQuestions(q => q.GetLatestFromRecentsList(skip, take));
		}

		public void StoreQuestion(Question question)
		{
			using (var redis = RedisManager.GetClient())
			using (var redisQuestions = redis.GetTypedClient<Question>())
			{
				if (question.Id == default(long))
					question.Id = redisQuestions.GetNextSequence();

				redisQuestions.Store(question);
				redisQuestions.AddToRecentsList(question);
			}
		}

		public void StoreAnswer(Answer answer)
		{
			using (var redis = RedisManager.GetReadOnlyClient())
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
			return ExecRedisQuestions(q => q.GetRelatedEntities<Answer>(questionId));
		}

		public void VoteQuestionUp(long userId, long questionId)
		{
			using (var redis = RedisManager.GetClient())
			{
				redis.AddItemToSet(UpVotesPrefix + questionId, userId.ToString());
				redis.RemoveItemFromSet(DownVotesPrefix + questionId, userId.ToString());
			}
		}

		public void VoteQuestionDown(long userId, long questionId)
		{
			using (var redis = RedisManager.GetClient())
			{
				redis.AddItemToSet(DownVotesPrefix + questionId, userId.ToString());
				redis.RemoveItemFromSet(UpVotesPrefix + questionId, userId.ToString());
			}
		}

		public QuestionStat GetQuestionStats(long questionId)
		{
			using (var redis = RedisManager.GetReadOnlyClient())
			{
				var result = new QuestionStat
				{
					VotesUpCount = redis.GetSetCount(UpVotesPrefix + questionId),
					VotesDownCount = redis.GetSetCount(DownVotesPrefix + questionId)
				};
				result.VotesTotal = result.VotesUpCount - result.VotesDownCount;
				return result;
			}
		}
	}

}