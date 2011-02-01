using System.IO;
using RestFiles.ServiceModel.Operations;
using ServiceStack.Common.Utils;
using ServiceStack.ServiceInterface;

namespace RestFiles.ServiceInterface
{
	public class ResetFilesService
		: RestServiceBase<ResetFiles>
	{
		public AppConfig Config { get; set; }

		public override object OnGet(ResetFiles request)
		{
			var rootDir = Config.RootDirectory;

			if (Directory.Exists(rootDir))
			{
				Directory.Delete(rootDir, true);
			}

			Directory.CreateDirectory(rootDir);

			foreach (var filePath in Directory.GetFiles("~/".MapHostAbsolutePath()))
			{
				if (filePath.EndsWith(".cs") || filePath.EndsWith(".html"))
				{
					File.Copy(filePath, Path.Combine(rootDir, Path.GetFileName(filePath)));
				}
			}

			var servicesDir = Path.Combine(rootDir, "services");
			foreach (var filePath in Directory.GetFiles("~/../RestFiles.ServiceInterface/".MapHostAbsolutePath()))
			{
				if (filePath.EndsWith("Service.cs"))
				{
					File.Copy(filePath, Path.Combine(servicesDir, Path.GetFileName(filePath)));
				}
			}

			var testsDir = Path.Combine(rootDir, "tests");
			foreach (var filePath in Directory.GetFiles("~/../RestFiles.ServiceInterface/".MapHostAbsolutePath()))
			{
				if (filePath.EndsWith("Service.cs"))
				{
					File.Copy(filePath, Path.Combine(testsDir, Path.GetFileName(filePath)));
				}
			}

			var dtosDir = Path.Combine(rootDir, "dtos");
			foreach (var dirPath in Directory.GetDirectories("~/../RestFiles.ServiceModel/".MapHostAbsolutePath()))
			{
				Directory.CreateDirectory(dtosDir);

				var subDirPath = Path.Combine(dtosDir, Path.GetDirectoryName(dirPath));
				Directory.CreateDirectory(subDirPath);
				foreach (var filePath in Directory.GetFiles(dirPath))
				{
					File.Copy(filePath, Path.Combine(subDirPath, Path.GetFileName(filePath)));
				}
			}

			return new ResetFilesResponse();
		}

	}
}