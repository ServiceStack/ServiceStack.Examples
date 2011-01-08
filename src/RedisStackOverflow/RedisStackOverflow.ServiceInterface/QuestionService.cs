using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace RedisStackOverflow.ServiceInterface
{
	[DataContract]
	[RestService("/questions", "POST")]
	[RestService("/questions/{Id}")]
	public class Question
	{
		public Question()
		{
			this.Tags = new List<string>();
		}

		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public long UserId { get; set; }

		[DataMember]
		public string Title { get; set; }

		[DataMember]
		public string Content { get; set; }

		[DataMember]
		public DateTime CreatedDate { get; set; }

		[DataMember]
		public List<string> Tags { get; set; }
	}

	[DataContract]
	public class Answer
	{
		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public long QuestionId { get; set; }

		[DataMember]
		public long UserId { get; set; }

		[DataMember]
		public DateTime CreatedDate { get; set; }

		[DataMember]
		public string Content { get; set; }
	}

	[DataContract]
	public class AnswerResult
	{
		[DataMember]
		public Answer Answer { get; set; }

		[DataMember]
		public User User { get; set; }
	}

	[DataContract]
	public class QuestionResponse : IHasResponseStatus
	{
		[DataMember]
		public QuestionResult Result { get; set; }

		[DataMember]
		public ResponseStatus ResponseStatus { get; set; }
	}

	public class QuestionService
	: RestServiceBase<Question>
	{
		public IRepository Repository { get; set; }

		public override object OnGet(Question question)
		{
			return new QuestionResponse
			{
				Result = Repository.GetQuestion(question.Id),
			};
		}

		public override object OnPost(Question question)
		{
			Repository.StoreQuestion(question);
			return new QuestionResponse();
		}
	}
}