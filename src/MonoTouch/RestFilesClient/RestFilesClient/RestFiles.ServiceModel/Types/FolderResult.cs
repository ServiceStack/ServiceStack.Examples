using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RestFiles.ServiceModel.Types
{
	[DataContract]
	public class FolderResult
	{
		public FolderResult()
		{
			Folders = new List<Folder>();
			Files = new List<File>();
		}

		[DataMember]
		public List<Folder> Folders { get; set; }

		[DataMember]
		public List<File> Files { get; set; }
	}
}