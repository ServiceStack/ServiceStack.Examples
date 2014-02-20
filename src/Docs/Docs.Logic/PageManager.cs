using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ServiceStack;

namespace Docs.Logic
{
	public class PageManager
	{
		public static PageManager Instance = new PageManager();

		public List<Page> Pages { get; set; }
		public Dictionary<string, Page> PageMap { get; set; }
		public Dictionary<string, List<Page>> CategoriesMap { get; set; }
		public string BaseUrl;

		public void Init(string filePath, string baseUrl)
		{
			BaseUrl = baseUrl;
			if (File.Exists(filePath))
			{
				var json = File.ReadAllText(filePath);
                this.Pages = json.FromJson<List<Page>>();
				this.Pages.ForEach(x => x.FilePath = x.FilePath.MapServerPath());
			}
			else
			{
				//First Run: Pages.json doesn't exist let's create and download all remote content
				this.Pages = SeedData.Pages;
				var basePath = "~".MapServerPath();
				Pages.ForEach(x => x.SaveRemoteContent(basePath, overwrite: false));
				File.WriteAllText(filePath, Pages.ToJson());
			}

			this.PageMap = new Dictionary<string, Page>(StringComparer.CurrentCultureIgnoreCase);
			Pages.Where(x => !x.FilePath.IsNullOrEmpty()).Each(x => PageMap[x.FilePath] = x);

			CategoriesMap = new Dictionary<string, List<Page>>(StringComparer.CurrentCultureIgnoreCase);
			foreach (var page in Pages)
			{
				List<Page> pages;
				if (!CategoriesMap.TryGetValue(page.Category, out pages))
				{
					pages = new List<Page>();
					CategoriesMap[page.Category] = pages;
				}

				pages.Add(page);
			}
		}

	}
}