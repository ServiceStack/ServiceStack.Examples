using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Redis;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace ServiceStack.Questions.ServiceInterface
{
	[RestService("/questions")]
	[DataContract]
	public class Questions {}

	[DataContract]
	public class QuestionsResponse
	{
		[DataMember]
		public List<Question> Results { get; set; }
	}

	public class QuestionsService 
		: RestServiceBase<Questions>
	{
		public IRedisClientsManager RedisManager { get; set; } 

		public override object OnGet(Questions request)
		{
			using (var redis = RedisManager.GetReadOnlyClient())
			using (var redisQuestions = redis.GetTypedClient<Question>())
			{
				return new QuestionsResponse { Results = new List<Question>(redisQuestions.GetAll()) };
			}
		}
	}
}