using System;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace RedisStackOverflow.ServiceInterface
{
	[DataContract]
	[RestService("/answers")]
	public class Answers
	{
		[DataMember]
		public int UserId { get; set; }

		[DataMember]
		public int QuestionId { get; set; }

		[DataMember]
		public string Content { get; set; }
	}

	[DataContract]
	public class AnswersResponse : IHasResponseStatus
	{
		//For automatic injection of service exceptions
		[DataMember] public ResponseStatus ResponseStatus { get; set; }
	}

	public class AnswersService
	: RestServiceBase<Answers>
	{
		public IRepository Repository { get; set; }

		public override object OnPost(Answers request)
		{
			Repository.StoreAnswer(new Answer
			{
				UserId = request.UserId,
				QuestionId = request.QuestionId,
				Content = request.Content
			});
			return new AnswersResponse();
		}
	}
}