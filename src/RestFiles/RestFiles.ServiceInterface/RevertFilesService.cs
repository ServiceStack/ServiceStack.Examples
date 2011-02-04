using System.IO;
using RestFiles.ServiceModel.Operations;
using ServiceStack.Common.Utils;
using ServiceStack.ServiceInterface;

namespace RestFiles.ServiceInterface
{
	public class RevertFilesService
		: RestServiceBase<RevertFiles>
	{
		public AppConfig Config { get; set; }

		public override object OnPost(RevertFiles request)
		{
			var rootDir = Config.RootDirectory;

			if (Directory.Exists(rootDir))
			{
				Directory.Delete(rootDir, true);
			}

			Directory.CreateDirectory(rootDir);

			foreach (var filePath in Directory.GetFiles("~/".MapHostAbsolutePath()))
			{
				if (!filePath.EndsWith(".cs")
					&& !filePath.EndsWith(".htm")
					&& !filePath.EndsWith(".md")) continue;

				var fileName = Path.GetFileName(filePath);
				if (filePath.EndsWith(".cs")) fileName += ".txt";
				File.Copy(filePath, Path.Combine(rootDir, fileName));
			}

			var servicesDir = Path.Combine(rootDir, "services");
			Directory.CreateDirectory(servicesDir);
			foreach (var filePath in Directory.GetFiles("~/../RestFiles.ServiceInterface/".MapHostAbsolutePath()))
			{
				if (!filePath.EndsWith("Service.cs")) continue;
				
				File.Copy(filePath, Path.Combine(servicesDir, Path.GetFileName(filePath) + ".txt"));
			}

			var testsDir = Path.Combine(rootDir, "tests");
			Directory.CreateDirectory(testsDir);
			foreach (var filePath in Directory.GetFiles("~/../RestFiles.Tests/".MapHostAbsolutePath()))
			{
				if (!filePath.EndsWith(".cs")) continue;
				
				File.Copy(filePath, Path.Combine(testsDir, Path.GetFileName(filePath) + ".txt"));
			}

			var dtosDir = Path.Combine(rootDir, "dtos");

			var opsDtoPath = Path.Combine(dtosDir, "Operations");
			Directory.CreateDirectory(opsDtoPath);
			foreach (var filePath in Directory.GetFiles("~/../RestFiles.ServiceModel/Operations/".MapHostAbsolutePath()))
			{
				File.Copy(filePath, Path.Combine(opsDtoPath, Path.GetFileName(filePath) + ".txt"));
			}
			var typesDtoPath = Path.Combine(dtosDir, "Types");
			Directory.CreateDirectory(typesDtoPath);
			foreach (var filePath in Directory.GetFiles("~/../RestFiles.ServiceModel/Types/".MapHostAbsolutePath()))
			{
				File.Copy(filePath, Path.Combine(typesDtoPath, Path.GetFileName(filePath) + ".txt"));
			}

			return new RevertFilesResponse();
		}

	}
}