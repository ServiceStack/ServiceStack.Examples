using System;
using System.Collections.Generic;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace RedisStackOverflow.ServiceModel
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
        public long VotesUpCount { get; set; }
        public long VotesDownCount { get; set; }
        public long VotesTotal { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    public class QuestionStatsResponse
    {
        public QuestionStat Result { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service request (i.e. the Request DTO).
    /// </summary>  
    [Route("/questions", "POST")]
    [Route("/questions/{Id}")]
    public class Question
    {
        public Question()
        {
            this.Tags = new List<string>();
        }

        public long Id { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string> Tags { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    public class QuestionResponse : IHasResponseStatus
    {
        public QuestionResult Result { get; set; }

        /// <summary>
        /// Gets or sets the ResponseStatus. The built-in Ioc used with ServiceStack autowires this property with service exceptions.
        /// </summary>
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service request (i.e. the Request DTO).
    /// </summary>  
    [Route("/questions", "GET")]
    [Route("/questions/page/{Page}")]
    [Route("/questions/tagged/{Tag}")]
    [Route("/users/{UserId}/questions")]
    public class Questions
    {
        public int? Page { get; set; }
        public string Tag { get; set; }
        public long? UserId { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    public class QuestionsResponse
    {
        public QuestionsResponse()
        {
            //Comment this out if you wish to receive the response status.
            this.ResponseStatus = new ResponseStatus();
        }

        public List<QuestionResult> Results { get; set; }

        /// <summary>
        /// Gets or sets the ResponseStatus. The built-in Ioc used with ServiceStack autowires this property with service exceptions.
        /// </summary>
        public ResponseStatus ResponseStatus { get; set; }
    }

    public class QuestionResult
    {
        public QuestionResult()
        {
            this.Answers = new List<AnswerResult>();
        }

        public Question Question { get; set; }
        public User User { get; set; }
        public long AnswersCount { get; set; }
        public long VotesUpCount { get; set; }
        public long VotesDownCount { get; set; }
        public List<AnswerResult> Answers { get; set; }
    }


}
