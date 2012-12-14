using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Markdown;

namespace Docs.Logic
{
	/// <summary>
	/// Custom base page for all Markdown pages.
	/// Provide util methods and setup default variables.
	/// </summary>
	public class CustomMarkdownPage : MarkdownViewBase
	{
		public override void OnLoad()
		{
			Page page;
			var pageManager = AppHost.TryResolve<PageManager>();
			pageManager.PageMap.TryGetValue(MarkdownPage.FilePath, out page);

			object cat;
			this.ScopeArgs.TryGetValue("Category", out cat);
			
			if (page == null)
				page = new Page { Category = cat != null ? cat.ToString() : "Framework" };

			if (!this.ScopeArgs.ContainsKey("Title"))
				this.ScopeArgs.Add("Title", page.Name);

			if (!this.ScopeArgs.ContainsKey("Category"))
				this.ScopeArgs.Add("Category", page.Category);

			this.ScopeArgs.Add("PagesMenu", GetPagesMenu(page));

			var lastModified = MarkdownPage.LastModified.GetValueOrDefault(DateTime.Now);
			this.ScopeArgs.Add("ModifiedDay", lastModified.Day);
			this.ScopeArgs.Add("ModifiedMonth", lastModified.ToString("MMM"));
		}

		public string GetPagesMenu(Page selectedPage)
		{
			var sb = new StringBuilder();
			sb.Append("<ul>\n");
			foreach (var kvp in PageManager.Instance.CategoriesMap)
			{
				var category = kvp.Key;
				var categoryPages = kvp.Value;
				var categoryUrl = PageManager.Instance.BaseUrl + "category/" + category;

				if (category == selectedPage.Category)
					sb.AppendFormat("<li><b><a href='{0}'>{1}</a></b> ({2})\n", categoryUrl, category, categoryPages.Count);
				else
					sb.AppendFormat("<li><a href='{0}'>{1}</a> ({2})\n", categoryUrl, category, categoryPages.Count);

				if (category == selectedPage.Category)
				{
					sb.Append("<ul class='children'>\n");
					foreach (var page in categoryPages)
					{
						var pageUrl = PageManager.Instance.BaseUrl + page.RelativeUrl;
						var cls = selectedPage.FilePath == page.FilePath ? " class='active'" : "";
						sb.AppendFormat("<li{0}><a href='{1}'>{2}</a></li>\n", cls, pageUrl, page.Name);
					}
					sb.Append("</ul>\n");
				}

				sb.Append("</li>\n");
			}
			sb.Append("</ul>\n");
			return sb.ToString();
		}

		public int Len<T>(IEnumerable<T> items)
		{
			return items.Count();
		}
	}
}
