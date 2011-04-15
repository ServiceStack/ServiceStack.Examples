using System.Runtime.Serialization;
using ServiceStack.Examples.ServiceModel.Types;
using ServiceStack.ServiceInterface.ServiceModel;

namespace ServiceStack.Examples.ServiceModel.Operations
{
	public class StoreLogs
	{
		public ArrayOfLogger Loggers { get; set; }
	}

	public class StoreLogsResponse
	{
		public StoreLogsResponse()
		{
			this.ResponseStatus = new ResponseStatus();

			this.ExistingLogs = new ArrayOfLogger();
		}

		public ArrayOfLogger ExistingLogs { get; set; }

		public ResponseStatus ResponseStatus { get; set; }
	}
}