using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace RedisStackOverflow.ServiceInterface
{
	[DataContract]
	[RestService("/users/{UserId}/questionvotes/{QuestionId}")]
	[RestService("/users/{UserId}/answervotes/{AnswerId}")]
	public class UserVotes
	{
		[DataMember]
		public long UserId { get; set; }

		[DataMember]
		public long? QuestionId { get; set; }

		[DataMember]
		public long? AnswerId { get; set; }

		[DataMember]
		public string Direction { get; set; }
	}

	[DataContract]
	public class UserVotesResponse { }

	public class UserVotesService
	: RestServiceBase<UserVotes>
	{
		public IRepository Repository { get; set; }

		public override object OnPost(UserVotes request)
		{
			var direction = request.Direction ?? "up";
			var voteUp = direction.ToLower() != "down";

			if (request.QuestionId.HasValue)
			{
				if (voteUp)
					Repository.VoteQuestionUp(request.UserId, request.QuestionId.Value);
				else
					Repository.VoteQuestionDown(request.UserId, request.QuestionId.Value);
			}
			else if (request.AnswerId.HasValue)
			{
				if (voteUp)
					Repository.VoteAnswerUp(request.UserId, request.AnswerId.Value);
				else
					Repository.VoteAnswerDown(request.UserId, request.AnswerId.Value);
			}

			return new UserVotesResponse();
		}
	}
}