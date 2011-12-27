using System;
using System.IO;

namespace RestFiles.ServiceInterface.Support
{
	public static class FileExtensions
	{
		public static string GetSafePath(this string filePath)
		{
			if (string.IsNullOrEmpty(filePath)) return string.Empty;

			//Strip invalid chars
			foreach (var invalidChar in Path.GetInvalidPathChars())
			{
				filePath = filePath.Replace(invalidChar.ToString(), String.Empty);
			}

			return filePath
			.TrimStart('.', '/', '\\')					//Remove illegal chars at the start
			.Replace('\\', '/')							//Switch all to use the same seperator
			.Replace("../", String.Empty)				//Remove access to top-level directories anywhere else 
			.Replace('/', Path.DirectorySeparatorChar); //Switch all to use the OS seperator
		}
	}
}