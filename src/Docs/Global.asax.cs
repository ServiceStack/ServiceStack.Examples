using System;
using Docs.Logic;
using Funq;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.ServiceHost;
using ServiceStack.WebHost.Endpoints;

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

			SetConfig(new EndpointHostConfig {
				WebHostUrl = baseUrl,                          //replaces ~/ with Url
				MarkdownBaseType = typeof(CustomMarkdownPage), //set custom base for all Markdown pages
			});
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