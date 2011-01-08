using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Redis;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace ServiceStack.Questions.ServiceInterface
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
		public string Content { get; set; }
	}

	[DataContract]
	public class QuestionResponse
	{
		[DataMember]
		public Question Question { get; set; }

		[DataMember]
		public List<Answer> Answers { get; set; }

		[DataMember]
		public List<User> Users { get; set; }
	}

	public class QuestionService
		: RestServiceBase<Question>
	{
		public IRepository Repository { get; set; }

		public override object OnGet(Question request)
		{
			var response = new QuestionResponse
			{
				Question = Repository.GetQuestion(request.Id),
				Answers = Repository.GetAnswersForQuestion(request.Id)
			};

			var userIds = new HashSet<long> { response.Question.UserId };
			response.Answers.ForEach(x => userIds.Add(x.UserId));

			response.Users = Repository.GetUsersByIds(userIds);

			return response;
		}

		public override object OnPost(Question question)
		{
			Repository.StoreQuestion(question);
			return new QuestionResponse();
		}
	}

}