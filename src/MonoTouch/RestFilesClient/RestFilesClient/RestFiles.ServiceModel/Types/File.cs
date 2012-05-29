using System;
using System.Runtime.Serialization;

namespace RestFiles.ServiceModel.Types
{
	[DataContract]
	public class File
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Extension { get; set; }

		[DataMember]
		public long FileSizeBytes { get; set; }

		[DataMember]
		public DateTime ModifiedDate { get; set; }

		[DataMember]
		public bool IsTextFile { get; set; }
	}
}