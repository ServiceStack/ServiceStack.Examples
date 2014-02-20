using RedisStackOverflow.ServiceModel;
using ServiceStack;

namespace RedisStackOverflow.ServiceInterface
{
    /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class UserService : Service
    {
        /// <summary>
        /// Gets or sets the repository. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
        public IRepository Repository { get; set; }

        public UserResponse Post(User request)
        {
            return new UserResponse
            {
                Result = Repository.GetOrCreateUser(request),
            };
        }

        public object Get(UserStats request)
        {
            return new UserStatsResponse {
                Result = Repository.GetUserStats(request.UserId)
            };
        }

        public object Post(UserVotes request)
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