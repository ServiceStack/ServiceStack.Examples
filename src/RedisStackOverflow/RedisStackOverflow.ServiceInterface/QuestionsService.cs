using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Common.Extensions;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace RedisStackOverflow.ServiceInterface
{
	[RestService("/questions", "GET")]
	[RestService("/questions/page/{Page}")]
	[RestService("/questions/tagged/{Tag}")]
	[RestService("/users/{UserId}/questions")]
	[DataContract]
	public class Questions
	{
		[DataMember]
		public int? Page { get; set; }

		[DataMember]
		public string Tag { get; set; }

		[DataMember]
		public long? UserId { get; set; }
	}

	[DataContract]
	public class QuestionsResponse
	{
		public QuestionsResponse()
		{
			this.ResponseStatus = new ResponseStatus();
		}

		[DataMember]
		public List<QuestionResult> Results { get; set; }

		[DataMember]
		public ResponseStatus ResponseStatus { get; set; }
	}

	[DataContract]
	public class QuestionResult
	{
		public QuestionResult()
		{
			this.Answers = new List<AnswerResult>();
		}

		[DataMember]
		public Question Question { get; set; }

		[DataMember]
		public User User { get; set; }

		[DataMember]
		public int AnswersCount { get; set; }

		[DataMember]
		public int VotesUpCount { get; set; }

		[DataMember]
		public int VotesDownCount { get; set; }

		[DataMember]
		public List<AnswerResult> Answers { get; set; }
	}

	public class QuestionsService
	: RestServiceBase<Questions>
	{
		public IRepository Repository { get; set; }

		public override object OnGet(Questions request)
		{
			if (!request.Tag.IsNullOrEmpty())
				return new QuestionsResponse { Results = Repository.GetQuestionsTaggedWith(request.Tag) };

			if (request.UserId.HasValue)
				return new QuestionsResponse { Results = Repository.GetQuestionsByUser(request.UserId.Value) };

			var pageOffset = request.Page.GetValueOrDefault(0) * 10;
			return new QuestionsResponse { Results = Repository.GetRecentQuestionResults(pageOffset, pageOffset + 10) };
		}
	}
}