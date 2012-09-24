using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Common.Extensions;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace RedisStackOverflow.ServiceInterface
{
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
        public int AnswersCount { get; set; }
        public int VotesUpCount { get; set; }
        public int VotesDownCount { get; set; }
        public List<AnswerResult> Answers { get; set; }
    }

    /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class QuestionsService : RestServiceBase<Questions>
    {
        /// <summary>
        /// Gets or sets the repository. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
        public IRepository Repository { get; set; }

        public override object OnGet(Questions request)
        {
            if (!request.Tag.IsNullOrEmpty())
                return new QuestionsResponse { Results = Repository.GetQuestionsTaggedWith(request.Tag) };

            if (request.UserId.HasValue)
                return new QuestionsResponse { Results = Repository.GetQuestionsByUser(request.UserId.Value) };

            var pageOffset = request.Page.GetValueOrDefault(0) * 10;
            return new QuestionsResponse { Results = Repository.GetRecentQuestionResults(pageOffset, pageOffset + 10) };
        }
    }
}