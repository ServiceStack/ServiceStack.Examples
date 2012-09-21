using System;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace RedisStackOverflow.ServiceInterface
{
    /// <summary>
    /// Define your ServiceStack web service request (i.e. the Request DTO).
    /// </summary>   
    [Route("/answers")]
    public class Answers
    {		
        public int UserId { get; set; }		
        public int AnswerId { get; set; }		
        public int QuestionId { get; set; }		
        public string Content { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    public class AnswersResponse : IHasResponseStatus
    {
        /// <summary>
        /// Gets or sets the ResponseStatus. The built-in Ioc used with ServiceStack autowires this property with service exceptions.
        /// </summary>
         public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class AnswersService : RestServiceBase<Answers>
    {
        /// <summary>
        /// Gets or sets the repository. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
        public IRepository Repository { get; set; }

        public override object OnPost(Answers request)
        {
            Repository.StoreAnswer(new Answer
            {
                UserId = request.UserId,
                QuestionId = request.QuestionId,
                Content = request.Content
            });
            return new AnswersResponse();
        }

        public override object OnDelete(Answers request)
        {
            Repository.DeleteAnswer(request.QuestionId, request.AnswerId);
            return new AnswersResponse();
        }
    }
}