using System.Collections.Generic;
using System.IO;
using Docs;
using Docs.Logic;
using ServiceStack.Text;

namespace ConsoleUtil
{
	class Program
	{
		public static List<Page> Pages { get; set; }

		static void Main(string[] args)
		{
			var filePath = @"C:\src\ServiceStack.Examples\src\Docs\Pages.json";
			if (File.Exists(filePath))
			{
				var json = File.ReadAllText(filePath);
				Pages = JsonSerializer.DeserializeFromString<List<Page>>(json);

				var webHostUrl = "http://servicestack.net/docs/";
				foreach (var page in Pages)
				{
					//ReplaceOldLinks(webHostUrl, page);
					var contents = page.GetContent();
					contents = contents.Replace(webHostUrl, "~/");
					Save(page, contents);
				}
			}
		}

		public static void ReplaceOldLinks(string baseUrl, Page srcPage)
		{
			var content = srcPage.GetContent();
			if (content == null) return;

			foreach (var page in Pages)
			{
				var realSrc = page.Src;
				if (realSrc.EndsWith("/wiki/Home"))
					realSrc = realSrc.Replace("/wiki/Home", "/wiki");
				else if (realSrc.EndsWith("/blob/master/README.md"))
					realSrc = realSrc.Replace("/blob/master/README.md", "");

				var realSrcLink = "(" + realSrc + ")";
				if (content.IndexOf(realSrcLink) == -1) continue;

				var newUrl = baseUrl + page.Category.SafeName() + "/" + page.Slug;

				var newUrlLink = "(" + newUrl + ")";
				content = content.Replace(realSrcLink, newUrlLink);
			}

			content = content.Replace(
				"(https://github.com/ServiceStack/ServiceStack.Redis/wiki/Caching)",
				"(http://servicestack.net/docs/framework/caching-options)");

			Save(srcPage, content);
		}

		public static void Save(Page srcPage, string contents)
		{
			var cateogryDir = Path.Combine(@"C:\src\ServiceStack.Examples\src\Docs", srcPage.Category.SafeName());

			if (!Directory.Exists(cateogryDir))
				Directory.CreateDirectory(cateogryDir);

			srcPage.FilePath = Path.Combine(cateogryDir, srcPage.Slug + ".md");
			File.WriteAllText(srcPage.FilePath, contents);
		}

	}

}
