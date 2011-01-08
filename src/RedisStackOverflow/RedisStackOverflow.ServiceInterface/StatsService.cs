using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace ServiceStack.Questions.ServiceInterface
{
	[DataContract]
	[RestService("/stats")]
	public class Stats {}

	[DataContract]
	public class SiteStats
	{
		public SiteStats()
		{
			this.TopTags = new List<Tag>();
		}

		[DataMember]
		public int QuestionsCount { get; set; }

		[DataMember]
		public int AnswersCount { get; set; }

		[DataMember]
		public List<Tag> TopTags { get; set; }
	}

	[DataContract]
	public class Tag
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int Score { get; set; }
	}

	[DataContract]
	public class StatsResponse
	{
		[DataMember]
		public SiteStats Result { get; set; }
	}

	public class StatsService
		: RestServiceBase<Stats>
	{
		public IRepository Repository { get; set; }

		public override object OnGet(Stats request)
		{
			return new StatsResponse
			{
				Result = Repository.GetSiteStats()
			};
		}
	}
}