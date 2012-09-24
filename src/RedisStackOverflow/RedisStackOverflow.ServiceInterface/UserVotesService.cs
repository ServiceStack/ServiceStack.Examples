using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace RedisStackOverflow.ServiceInterface
{
    /// <summary>
    /// Define your ServiceStack web service request (i.e. the Request DTO).
    /// </summary> 
    [Route("/users/{UserId}/questionvotes/{QuestionId}")]
    [Route("/users/{UserId}/answervotes/{AnswerId}")]
    public class UserVotes
    {		
        public long UserId { get; set; }		
        public long? QuestionId { get; set; }		
        public long? AnswerId { get; set; }		
        public string Direction { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    public class UserVotesResponse { }

    /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class UserVotesService : RestServiceBase<UserVotes>
    {
        /// <summary>
        /// Gets or sets the repository. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
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