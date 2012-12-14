using System.Collections.Generic;

namespace RestFiles.ServiceModel.Types
{	
	public class FolderResult
	{
		public FolderResult()
		{
			Folders = new List<Folder>();
			Files = new List<File>();
		}
		
		public List<Folder> Folders { get; set; }		
		public List<File> Files { get; set; }
	}
}