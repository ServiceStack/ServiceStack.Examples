using System.ComponentModel;
using System.Runtime.Serialization;
using RestFiles.ServiceModel.Types;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace RestFiles.ServiceModel.Operations
{
	[Description("GET the File or Directory info at {Path}\n"
               + "POST multipart/formdata to upload a new file to any {Path} in the /ReadWrite folder\n"
               + "PUT {TextBody} to replace the contents of a text file in the /ReadWrite folder\n")]
	[RestService("/files")]
	[RestService("/files/{Path*}")]
	[DataContract]
	public class Files
	{
		[DataMember]
		public string Path { get; set; }

		[DataMember]
		public string TextBody { get; set; }
	}

	[DataContract]
	public class FilesResponse : IHasResponseStatus
	{
		[DataMember]
		public FolderResult Directory { get; set; }

		[DataMember]
		public FileResult File { get; set; }

		//Auto inject and serialize web service exceptions
		[DataMember] public ResponseStatus ResponseStatus { get; set; }
	}
}