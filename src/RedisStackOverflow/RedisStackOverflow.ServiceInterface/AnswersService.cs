using RedisStackOverflow.ServiceModel;
using ServiceStack.ServiceInterface;

namespace RedisStackOverflow.ServiceInterface
{

    /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class AnswersService : Service
    {
        /// <summary>
        /// Gets or sets the repository. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
        public IRepository Repository { get; set; }

        public void Post(Answers request)
        {
            Repository.StoreAnswer(new Answer
            {
                UserId = request.UserId,
                QuestionId = request.QuestionId,
                Content = request.Content
            });
        }

        public void Delete(Answers request)
        {
            Repository.DeleteAnswer(request.QuestionId, request.AnswerId);
        }
    }
}