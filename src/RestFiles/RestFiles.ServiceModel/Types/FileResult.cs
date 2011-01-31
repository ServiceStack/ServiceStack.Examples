using System;
using System.Runtime.Serialization;

namespace RestFiles.ServiceModel.Types
{
	[DataContract]
	public class FileResult
	{
		[DataMember]
		public string Contents { get; set; }

		[DataMember]
		public DateTime CreatedDate { get; set; }

		[DataMember]
		public DateTime LastModifiedDate { get; set; }
	}
}