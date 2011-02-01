using System;
using System.Runtime.Serialization;

namespace RestFiles.ServiceModel.Types
{
	[DataContract]
	public class Folder
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public DateTime ModifiedDate { get; set; }

		[DataMember]
		public int FileCount { get; set; }
	}
}