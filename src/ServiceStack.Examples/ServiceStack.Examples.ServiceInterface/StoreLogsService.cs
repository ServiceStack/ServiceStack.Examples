using ServiceStack.Examples.ServiceModel;
using ServiceStack.Examples.ServiceModel.Types;
using ServiceStack.OrmLite;

namespace ServiceStack.Examples.ServiceInterface
{
    public class StoreLogsService : Service
    {
        public object Any(StoreLogs request)
        {
            if (!request.Loggers.IsNullOrEmpty()) { Db.SaveAll(request.Loggers); }

            return new StoreLogsResponse
            {
                ExistingLogs = new ArrayOfLogger(Db.Select<Logger>())
            };
        }
    }
}