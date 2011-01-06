using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace ServiceStack.Questions.ServiceInterface
{
	[RestService("/questions", "GET")]
	[RestService("/questions/{Page}")]
	[DataContract]
	public class Questions
	{
		[DataMember]
		public int? Page { get; set; }
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
	}


	public class QuestionsService
		: RestServiceBase<Questions>
	{
		public IRepository Repository { get; set; }

		public override object OnGet(Questions request)
		{
			var pageOffset = request.Page.GetValueOrDefault(0) * 10;
			return new QuestionsResponse { Results = Repository.GetRecentQuestionResults(pageOffset, pageOffset + 10) };
		}
	}
}