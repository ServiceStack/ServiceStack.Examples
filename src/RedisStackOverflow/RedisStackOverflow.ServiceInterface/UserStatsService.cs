using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace RedisStackOverflow.ServiceInterface
{
	[DataContract]
	[RestService("/users/{UserId}/stats")]
	public class UserStats
	{
		[DataMember]
		public long UserId { get; set; }
	}

	[DataContract]
	public class UserStat
	{
		[DataMember]
		public long UserId { get; set; }

		[DataMember]
		public int QuestionsCount { get; set; }

		[DataMember]
		public int AnswersCount { get; set; }
	}

	[DataContract]
	public class UserStatsResponse
	{
		[DataMember]
		public UserStat Result { get; set; }
	}

	public class UserStatsService
		: RestServiceBase<UserStats>
	{
		public IRepository Repository { get; set; }

		public override object OnGet(UserStats request)
		{
			return new UserStatsResponse
			{
				Result = Repository.GetUserStats(request.UserId)
			};
		}

	}
}