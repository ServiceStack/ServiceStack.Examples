using System;
using System.Collections;
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
				if (user.Id != default(long))
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
			//Use 'Exec<T>' extension method to easy access to: 'redis.GetTypedClient<Question>()'
			return RedisManager.Exec<Question>(q => q.GetAll()).ToList();
		}

		public List<Question> GetRecentQuestions(int skip, int take)
		{
			//Use 'Exec<T>' extension method to easy access to: 'redis.GetTypedClient<Question>()'
			return RedisManager.Exec<Question>(q => q.GetLatestFromRecentsList(skip, take));
		}

		public void StoreQuestion(Question question)
		{
			RedisManager.Exec<Question>(redisQuestions => 
			{
				if (question.Id == default(long))
					question.Id = redisQuestions.GetNextSequence();

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