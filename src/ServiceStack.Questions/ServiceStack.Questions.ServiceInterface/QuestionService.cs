using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Redis;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace ServiceStack.Questions.ServiceInterface
{
	[DataContract]
	[RestService("/question")]
	[RestService("/questions/{Id}")]
	public class Question
	{
		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public string UserId { get; set; }

		[DataMember]
		public string Title { get; set; }

		[DataMember]
		public string Content { get; set; }
	}

	[DataContract]
	public class Answer
	{
		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public string QuestionId { get; set; }

		[DataMember]
		public string UserId { get; set; }

		[DataMember]
		public string Content { get; set; }
	}

	[DataContract]
	public class QuestionResponse
	{
		[DataMember]
		public Question Question { get; set; }

		[DataMember]
		public List<Answer> Answers { get; set; }
	}

	public class QuestionService
		: RestServiceBase<Question>
	{
		public IRedisClientsManager RedisManager { get; set; }

		public override object OnGet(Question request)
		{
			using (var redis = RedisManager.GetReadOnlyClient())
			using (var redisQuestions = redis.GetTypedClient<Question>())
			{
				return new QuestionResponse { Question = redisQuestions.GetById(request.Id.ToString()) };
			}
		}

		public override object OnPost(Question question)
		{
			using (var redis = RedisManager.GetReadOnlyClient())
			using (var redisQuestions = redis.GetTypedClient<Question>())
			{
				if (question.Id == default(long))
					question.Id = redisQuestions.GetNextSequence();

				redisQuestions.Store(question);

				return new QuestionResponse { Question = redisQuestions.GetById(question.Id.ToString()) };
			}
		}
	}

}