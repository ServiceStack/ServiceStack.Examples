using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace RedisStackOverflow.ServiceInterface
{
    /// <summary>
    /// Define your ServiceStack web service request (i.e. the Request DTO).
    /// </summary>
    [Route("/questions/{QuestionId}/stats")]
    public class QuestionStats
    {		
        public long QuestionId { get; set; }
    }
    
    public class QuestionStat
    {		
        public int VotesUpCount { get; set; }		
        public int VotesDownCount { get; set; }		
        public int VotesTotal { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    public class QuestionStatsResponse
    {		
        public QuestionStat Result { get; set; }
    }

    /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class QuestionStatService : RestServiceBase<QuestionStats>
    {
        /// <summary>
        /// Gets or sets the repository. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
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