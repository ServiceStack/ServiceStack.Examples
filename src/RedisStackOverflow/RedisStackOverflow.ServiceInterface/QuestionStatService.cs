using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace RedisStackOverflow.ServiceInterface
{
	[DataContract]
	[RestService("/questions/{QuestionId}/stats")]
	public class QuestionStats
	{
		[DataMember]
		public long QuestionId { get; set; }
	}

	[DataContract]
	public class QuestionStat
	{
		[DataMember]
		public int VotesUpCount { get; set; }

		[DataMember]
		public int VotesDownCount { get; set; }

		[DataMember]
		public int VotesTotal { get; set; }
	}

	[DataContract]
	public class QuestionStatsResponse
	{
		[DataMember]
		public QuestionStat Result { get; set; }
	}

	public class QuestionStatService
	: RestServiceBase<QuestionStats>
	{
		public IRepository Repository { get; set; }

		public override object OnPost(QuestionStats request)
		{
			return new QuestionStatsResponse
			{
				Result = Repository.GetQuestionStats(request.QuestionId)
			};
		}
	}
}