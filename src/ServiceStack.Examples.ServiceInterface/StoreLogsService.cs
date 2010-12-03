using ServiceStack.Examples.ServiceModel.Operations;
using ServiceStack.Examples.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.Text;

namespace ServiceStack.Examples.ServiceInterface
{
	public class StoreLogsService
		: IService<StoreLogs>
	{
		//Example of ServiceStack's built-in Funq IOC property injection
		public IDbConnectionFactory ConnectionFactory { get; set; }

		public object Execute(StoreLogs request)
		{
			using (var dbConn = ConnectionFactory.OpenDbConnection())
			using (var dbCmd = dbConn.CreateCommand())
			{
				if (!request.Loggers.IsNullOrEmpty()) dbCmd.SaveAll(request.Loggers);

				return new StoreLogsResponse
				{
					ExistingLogs = new ArrayOfLogger(dbCmd.Select<Logger>())
				};
			}
		}
	}

}