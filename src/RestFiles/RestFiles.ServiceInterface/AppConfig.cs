using System.Collections.Generic;
using System.IO;
using ServiceStack.Common.Utils;
using ServiceStack.Configuration;

namespace RestFiles.ServiceInterface
{
	public class AppConfig
	{
		public AppConfig()
		{
			this.TextFileExtensions = new List<string>();
			this.ExcludeDirectories = new List<string>();
		}

		public AppConfig(IResourceManager resources)
		{
			this.RootDirectory = resources.GetString("RootDirectory").MapHostAbsolutePath()
				.Replace('\\', Path.DirectorySeparatorChar);

			this.TextFileExtensions = resources.GetList("TextFileExtensions");
			this.ExcludeDirectories = resources.GetList("ExcludeDirectories");
		}

		public string RootDirectory { get; set; }

		public IList<string> TextFileExtensions { get; set; }

		public IList<string> ExcludeDirectories { get; set; }
	}
}