using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace RestFiles.ServiceModel.Operations
{
	[RestService("revertfiles")]
	[DataContract]
	public class RevertFiles { }
	
	[DataContract]
	public class RevertFilesResponse : IHasResponseStatus
	{
		public RevertFilesResponse()
		{
			this.ResponseStatus = new ResponseStatus();
		}

		[DataMember]
		public ResponseStatus ResponseStatus { get; set; }
	}
}