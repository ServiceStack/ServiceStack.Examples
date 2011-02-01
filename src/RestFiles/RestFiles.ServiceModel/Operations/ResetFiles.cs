using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace RestFiles.ServiceModel.Operations
{
	[RestService("resetfiles")]
	[DataContract]
	public class ResetFiles { }
	
	[DataContract]
	public class ResetFilesResponse : IHasResponseStatus
	{
		public ResetFilesResponse()
		{
			this.ResponseStatus = new ResponseStatus();
		}

		[DataMember]
		public ResponseStatus ResponseStatus { get; set; }
	}
}