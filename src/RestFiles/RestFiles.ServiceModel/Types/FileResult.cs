using System;

namespace RestFiles.ServiceModel.Types
{	
	public class FileResult
	{		
		public string Name { get; set; }		
		public string Extension { get; set; }		
		public long FileSizeBytes { get; set; }		
		public DateTime ModifiedDate { get; set; }		
		public bool IsTextFile { get; set; }		
		public string Contents { get; set; }
	}
}