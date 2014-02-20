using System.Collections.Generic;
using ServiceStack;

namespace RedisStackOverflow.ServiceModel
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

}