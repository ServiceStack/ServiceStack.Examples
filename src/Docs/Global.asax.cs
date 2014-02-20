using System;
using System.Linq;
using Docs.Logic;
using Funq;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Formats;

namespace Docs
{
	public class AppHost : AppHostBase
	{
		public AppHost()
			: base("ServiceStack Docs", typeof(PageService).Assembly) { }

		public override void Configure(Container container)
		{
			var baseUrl = ConfigUtils.GetAppSetting("WebHostUrl");
			PageManager.Instance.Init("~/Pages.json".MapServerPath(), baseUrl);

			container.Register(PageManager.Instance);

			Routes
				.Add<Page>("/pages")
				.Add<Page>("/pages/{Name}")
				.Add<Category>("/category/{Name}")
				.Add<Search>("/search")
				.Add<Search>("/search/{Query}");

			SetConfig(new HostConfig {
				WebHostUrl = baseUrl,                          //replaces ~/ with Url
				MarkdownBaseType = typeof(CustomMarkdownPage), //set custom base for all Markdown pages
			});

            var plugin = (MarkdownFormat)Plugins.First(x => x is MarkdownFormat);
		    var page = plugin.FindByPathInfo("/about");
		}
	}
	
	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			new AppHost().Init();
		}
	}
}