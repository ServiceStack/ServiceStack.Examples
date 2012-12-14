using RedisStackOverflow.ServiceModel;
using ServiceStack.Common.Extensions;
using ServiceStack.ServiceInterface;

namespace RedisStackOverflow.ServiceInterface
{
   /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class QuestionsService : Service
    {
        /// <summary>
        /// Gets or sets the repository. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
        public IRepository Repository { get; set; }

        public object Get(Questions request)
        {
            if (!request.Tag.IsNullOrEmpty())
                return new QuestionsResponse { Results = Repository.GetQuestionsTaggedWith(request.Tag) };

            if (request.UserId.HasValue)
                return new QuestionsResponse { Results = Repository.GetQuestionsByUser(request.UserId.Value) };

            var pageOffset = request.Page.GetValueOrDefault(0) * 10;
            return new QuestionsResponse { Results = Repository.GetRecentQuestionResults(pageOffset, pageOffset + 10) };
        }
        
        public object Get(Question question)
        {
            return new QuestionResponse {
                Result = Repository.GetQuestion(question.Id),
            };
        }

        public void Post(Question question)
        {
            Repository.StoreQuestion(question);
        }

        public void Delete(Question request)
        {
            Repository.DeleteQuestion(request.Id);
        }

        public object Post(QuestionStats request)
        {
            return new QuestionStatsResponse {
                Result = Repository.GetQuestionStats(request.QuestionId)
            };
        }
    }
}