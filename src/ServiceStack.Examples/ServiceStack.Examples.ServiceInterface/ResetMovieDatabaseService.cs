using ServiceStack.Data;
using ServiceStack.Examples.ServiceInterface.Support;
using ServiceStack.Examples.ServiceModel;
using ServiceStack.Examples.ServiceModel.Types;
using ServiceStack.OrmLite;

namespace ServiceStack.Examples.ServiceInterface
{
    /// <summary>
    /// An example of a very basic web service
    /// </summary>
    public class ResetMovieDatabaseService : Service
    {
        public IDbConnectionFactory ConnectionFactory { get; set; }

        public object Any(ResetMovieDatabase request)
        {
            Db.CreateTable<Movie>(true);
            Db.SaveAll(ConfigureDatabase.Top5Movies);

            return new ResetMovieDatabaseResponse();
        }
    }
}