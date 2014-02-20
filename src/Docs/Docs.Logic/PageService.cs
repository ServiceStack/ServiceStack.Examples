using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ServiceStack;

namespace Docs.Logic
{
	public class Page
	{
		public Page()
		{
			this.Tags = new List<string>();
		}

		public Page(string name, string src, string category, IEnumerable<string> tags)
		{
			Name = name;
			Src = src;
			Category = category;
			Tags = (tags ?? new List<string>()).ToList();
		}

		public string Name { get; set; }
		public string Slug { get; set; }
		public string Src { get; set; }
		public string FilePath { get; set; }
		public string Category { get; set; }
		public string Content { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public List<string> Tags { get; set; }

		public void SaveRemoteContent(string baseUrl, bool overwrite)
		{
			try
			{
				if (!overwrite && this.FilePath != null)
				{
					var fi = new FileInfo(this.FilePath);
					if (fi.Length > 0)
						return;
				}

				this.Slug = this.Name.SafeName();

				var remoteContent = GetRemoteContent();
				Save(baseUrl, remoteContent);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		public void LoadContent()
		{
			this.Content = GetContent();
		}

		public string GetContent()
		{
			return this.FilePath != null
				   ? File.ReadAllText(this.FilePath)
				   : GetRemoteContent();
		}

		public string GetRemoteContent()
		{
			var isGithubWiki = !this.Src.EndsWith(".md");
			if (isGithubWiki)
			{
				var markdownEditUrl = this.Src + "/_edit";
				var editPageContents = markdownEditUrl.GetStringFromUrl();
				return editPageContents.ExtractContents("<textarea", "name=\"wiki[body]\">", "</textarea>");
			}

			var markdownUrl = this.Src.Contains("/blob/")
							  ? this.Src.Replace("/blob/", "/raw/")
							  : this.Src;

            return markdownUrl.GetStringFromUrl();
		}

		public void Save(string baseUrl, string contents)
		{
			var cateogryDir = Path.Combine(baseUrl, this.Category.SafeName());

			if (!Directory.Exists(cateogryDir))
				Directory.CreateDirectory(cateogryDir);

			this.FilePath = Path.Combine(cateogryDir, this.Slug + ".md");
			File.WriteAllText(this.FilePath, contents);
		}

		public string RelativeUrl
		{
			get { return this.Category.SafeName() + "/" + this.Slug; }
		}

		public string AbsoluteUrl
		{
			get { return PageManager.Instance.BaseUrl + this.Category.SafeName() + "/" + this.Slug; }
		}
	}
	
	public class PageService : Service
	{
		public PageManager PageManager { get; set; }

		public object Get(Page request)
		{
			if (request.Name != null)
			{
				//Load Content for single pages
				var page = PageManager.Pages.FirstOrDefault(x => x.Name == request.Name);
				if (page == null) return null;
				var clone = page.DeepClone();
				clone.Content = page.GetContent();
				return clone;
			}

			return PageManager.Pages;
		}
	}
}