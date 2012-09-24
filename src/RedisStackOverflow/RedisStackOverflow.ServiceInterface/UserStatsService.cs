using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace RedisStackOverflow.ServiceInterface
{
    /// <summary>
    /// Define your ServiceStack web service request (i.e. the Request DTO).
    /// </summary> 
    [Route("/users/{UserId}/stats")]
    public class UserStats
    {		
        public long UserId { get; set; }
    }
    
    public class UserStat
    {		
        public long UserId { get; set; }		
        public int QuestionsCount { get; set; }		
        public int AnswersCount { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    public class UserStatsResponse
    {		
        public UserStat Result { get; set; }
    }

    /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class UserStatsService : RestServiceBase<UserStats>
    {
        /// <summary>
        /// Gets or sets the repository. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
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