using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace RedisStackOverflow.ServiceInterface
{
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
    
    public class Answer
    {		
        public long Id { get; set; }		
        public long QuestionId { get; set; }		
        public long UserId { get; set; }		
        public DateTime CreatedDate { get; set; }		
        public string Content { get; set; }
    }
    
    public class AnswerResult
    {		
        public Answer Answer { get; set; }		
        public User User { get; set; }
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
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class QuestionService : RestServiceBase<Question>
    {
        /// <summary>
        /// Gets or sets the repository. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
        public IRepository Repository { get; set; }

        public override object OnGet(Question question)
        {
            return new QuestionResponse
            {
                Result = Repository.GetQuestion(question.Id),
            };
        }

        public override object OnPost(Question question)
        {
            Repository.StoreQuestion(question);
            return new QuestionResponse();
        }

        public override object OnDelete(Question request)
        {
            Repository.DeleteQuestion(request.Id);

            return new QuestionResponse();
        }
    }
}