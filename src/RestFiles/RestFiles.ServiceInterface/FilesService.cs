using System;
using System.IO;
using System.Net;
using RestFiles.ServiceModel.Operations;
using RestFiles.ServiceModel.Types;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using File = System.IO.File;
using FileResult = RestFiles.ServiceModel.Types.FileResult;

/* For syntax highlighting and better readability view this file in GitHub at:
 * https://github.com/mythz/ServiceStack.Examples/blob/master/src/RestFiles/RestFiles.Tests/SyncRestClientTests.cs
 */

namespace RestFiles.ServiceInterface
{
	/// <summary>
	///  Contains the implementation of the Files Web Service.
	///  
	///  @Returns the Directory TextBody of the Files.Path
	///  
	///  Can also be called using the REST Urls below (default urls for XSP provided):
	/// 		        http://localhost/RestFiles/files/
	///  		- json: http://localhost/RestFiles/files/?format=json
	/// </summary>
	public class FilesService
		: RestServiceBase<Files>
	{
		public AppConfig Config { get; set; }

		public override object OnGet(Files request)
		{
			var targetFile = GetAndValidateExistingPath(request);

			var response = Directory.Exists(targetFile.FullName)
				? new FilesResponse { Directory = GetFolderResult(targetFile.FullName) }
				: new FilesResponse { File = GetFileResult(targetFile) };

			return response;
		}

		public override object OnPost(Files request)
		{
			var targetDir = GetPath(request);

			var isExistingFile = targetDir.Exists
				&& (targetDir.Attributes & FileAttributes.Directory) != FileAttributes.Directory;

			if (isExistingFile)
				throw new NotSupportedException(
				"POST only supports uploading new files. Use PUT to replace contents of an existing file");

			if (!Directory.Exists(targetDir.FullName))
			{
				Directory.CreateDirectory(targetDir.FullName);
			}

			foreach (var uploadedFile in base.RequestContext.Files)
			{
				var newFilePath = Path.Combine(targetDir.FullName, uploadedFile.FileName);
				uploadedFile.SaveTo(newFilePath);
			}

			return new FilesResponse();
		}

		public override object OnPut(Files request)
		{
			var targetFile = GetAndValidateExistingPath(request);

			if (!this.Config.TextFileExtensions.Contains(targetFile.Extension))
				throw new NotSupportedException("PUT Can only update text files, not: " + targetFile.Extension);

			if (request.TextBody == null)
				throw new ArgumentNullException("TextBody");

			File.WriteAllText(targetFile.FullName, request.TextBody);

			return new FilesResponse();
		}

		public override object OnDelete(Files request)
		{
			var targetFile = GetAndValidateExistingPath(request);
			
			File.Delete(targetFile.FullName);

			return new FilesResponse();
		}

		private FolderResult GetFolderResult(string targetPath)
		{
			var result = new FolderResult();

			foreach (var dirPath in Directory.GetDirectories(targetPath))
			{
				var dirInfo = new DirectoryInfo(dirPath);

				if (this.Config.ExcludeDirectories.Contains(dirInfo.Name)) continue;

				result.Folders.Add(new Folder
				{
					Name = dirInfo.Name,
					ModifiedDate = dirInfo.LastWriteTimeUtc,
					FileCount = dirInfo.GetFiles().Length
				});
			}

			foreach (var filePath in Directory.GetFiles(targetPath))
			{
				var fileInfo = new FileInfo(filePath);

				result.Files.Add(new ServiceModel.Types.File
				{
					Name = fileInfo.Name,
					Extension = fileInfo.Extension,
					FileSizeBytes = fileInfo.Length,
					ModifiedDate = fileInfo.LastWriteTimeUtc,
					IsTextFile = Config.TextFileExtensions.Contains(fileInfo.Extension),
				});
			}

			return result;
		}

		private FileInfo GetPath(Files request)
		{
			var targetPath = Path.Combine(this.Config.RootDirectory,
				GetSafePath(request.Path ?? string.Empty));

			return new FileInfo(targetPath);
		}

		private FileInfo GetAndValidateExistingPath(Files request)
		{
			var targetFile = GetPath(request);
			if (!targetFile.Exists && !Directory.Exists(targetFile.FullName))
				throw new HttpError(HttpStatusCode.NotFound,
					new FileNotFoundException("Could not find: " + request.Path));

			return targetFile;
		}

		private FileResult GetFileResult(FileInfo fileInfo)
		{
			var isTextFile = this.Config.TextFileExtensions.Contains(fileInfo.Extension);

			return new FileResult
			{
				Name = fileInfo.Name,
				Extension = fileInfo.Extension,
				FileSizeBytes = fileInfo.Length,
				IsTextFile = isTextFile,
				Contents = isTextFile ? File.ReadAllText(fileInfo.FullName) : null,
				ModifiedDate = fileInfo.LastWriteTimeUtc,
			};
		}

		public static string GetSafePath(string filePath)
		{
			//Strip invalid chars
			foreach (var invalidChar in Path.GetInvalidPathChars())
			{
				filePath = filePath.Replace(invalidChar.ToString(), string.Empty);
			}

			return filePath
			.TrimStart('.', '/', '\\')					//Remove illegal chars at the start
			.Replace('\\', '/')							//Switch all to use the same seperator
			.Replace("../", string.Empty)				//Remove access to top-level directories anywhere else 
			.Replace('/', Path.DirectorySeparatorChar); //Switch all to use the OS seperator
		}
	}
}