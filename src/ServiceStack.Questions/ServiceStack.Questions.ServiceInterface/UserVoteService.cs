using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace ServiceStack.Questions.ServiceInterface
{
	[DataContract]
	[RestService("/questions/{QuestionId}/votes/{UserId}/{Direction}")]
	public class UserVoteUp
	{
		[DataMember]
		public long UserId { get; set; }

		[DataMember]
		public long QuestionId { get; set; }

		[DataMember]
		public string Direction { get; set; }
	}

	[DataContract]
	public class UserVoteUpResponse { }

	public class UserVoteUpService
		: RestServiceBase<UserVoteUp>
	{
		public IRepository Repository { get; set; }

		public override object OnPost(UserVoteUp request)
		{
			var direction = request.Direction ?? "up";
			if (direction.ToLower() != "down")
				Repository.VoteQuestionUp(request.UserId, request.QuestionId);
			else
				Repository.VoteQuestionDown(request.UserId, request.QuestionId);

			return new UserVoteUpResponse();
		}
	}

}