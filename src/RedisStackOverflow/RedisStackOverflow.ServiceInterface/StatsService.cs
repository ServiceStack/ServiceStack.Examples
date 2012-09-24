using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace RedisStackOverflow.ServiceInterface
{
    /// <summary>
    /// Define your ServiceStack web service request (i.e. the Request DTO).
    /// </summary> 
    [Route("/stats")]
    public class Stats { }
    
    public class SiteStats
    {
        public SiteStats()
        {
            this.TopTags = new List<Tag>();
        }
        
        public int QuestionsCount { get; set; }		
        public int AnswersCount { get; set; }		
        public List<Tag> TopTags { get; set; }
    }
    
    public class Tag
    {		
        public string Name { get; set; }		
        public int Score { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    public class StatsResponse
    {		
        public SiteStats Result { get; set; }
    }

    /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class StatsService : RestServiceBase<Stats>
    {
        /// <summary>
        /// Gets or sets the repository. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
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