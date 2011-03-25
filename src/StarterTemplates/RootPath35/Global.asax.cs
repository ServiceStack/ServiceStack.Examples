using System;
using ServiceStack.WebHost.Endpoints;
using StarterTemplates.Common;

namespace RootPath35
{
	public class AppHost
		: AppHostBase
	{
		public AppHost()
			: base("StarterTemplate ASP.NET Host", typeof(HelloService).Assembly) { }

		public override void Configure(Funq.Container container)
		{
			container.Register(new TodoRepository());
		}
	}

	public class Global : System.Web.HttpApplication
	{
		void Application_Start(object sender, EventArgs e)
		{
			new AppHost().Init();
		}
	}
}
