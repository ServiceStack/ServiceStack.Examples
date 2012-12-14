using RedisStackOverflow.ServiceModel;
using ServiceStack.ServiceInterface;

namespace RedisStackOverflow.ServiceInterface
{
    /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class StatsService : Service
    {
        /// <summary>
        /// Gets or sets the repository. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
        public IRepository Repository { get; set; }

        public StatsResponse Get(Stats request)
        {
            return new StatsResponse
            {
                Result = Repository.GetSiteStats()
            };
        }
    }
}