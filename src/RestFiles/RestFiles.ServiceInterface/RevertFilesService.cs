using System.IO;
using System.Linq;
using RestFiles.ServiceModel;
using ServiceStack.Common.Extensions;
using ServiceStack.Common.Utils;
using ServiceStack.ServiceInterface;

namespace RestFiles.ServiceInterface
{
    /// <summary>
    /// Define your ServiceStack web service request (i.e. Request DTO).
    /// </summary>
    public class RevertFilesService : RestServiceBase<RevertFiles>
    {
        /// <summary>
        /// Gets or sets the AppConfig. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
        public AppConfig Config { get; set; }

        public override object OnPost(RevertFiles request)
        {
            var rootDir = Config.RootDirectory;

            if (Directory.Exists(rootDir))
            {
                Directory.Delete(rootDir, true);
            }

            CopyFiles(rootDir, "~/".MapHostAbsolutePath(), ".cs", ".htm", ".md");

            var servicesDir = Path.Combine(rootDir, "services");
            CopyFiles(servicesDir, "~/../RestFiles.ServiceInterface/".MapHostAbsolutePath(), "Service.cs");

            var testsDir = Path.Combine(rootDir, "tests");
            CopyFiles(testsDir, "~/../RestFiles.Tests/".MapHostAbsolutePath(), ".cs");

            var dtosDir = Path.Combine(rootDir, "dtos");

            var opsDtoPath = dtosDir;
            CopyFiles(opsDtoPath, "~/../RestFiles.ServiceModel/".MapHostAbsolutePath());

            var typesDtoPath = Path.Combine(dtosDir, "Types");
            CopyFiles(typesDtoPath, "~/../RestFiles.ServiceModel/Types/".MapHostAbsolutePath());

            return new RevertFilesResponse();
        }

        private static void CopyFiles(string path, string filesPath, params string[] excludedFiles)
        {
            Directory.CreateDirectory(path);
            var files = Directory.GetFiles(filesPath);
            foreach (var file in files)
            {
                if (excludedFiles.IsEmpty() || excludedFiles.Any(x => file.EndsWith(x)))
                {
                    var fileName = Path.GetFileName(file);
                    if (file.EndsWith(".cs"))
                    {
                        fileName += ".txt";
                    }
                    File.Copy(file, Path.Combine(path, fileName));
                }
            }
        }
    }
}