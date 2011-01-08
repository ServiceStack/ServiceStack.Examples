using System;
using Funq;
using ServiceStack.Questions.ServiceInterface;
using ServiceStack.Redis;
using ServiceStack.WebHost.Endpoints;

namespace ServiceStack.Questions.Host.Web
{
	public class AppHost
		: AppHostBase
	{
		public AppHost()
			: base("ServiceStack Questions", typeof(QuestionsService).Assembly) { }

		public override void Configure(Container container)
		{
			SetConfig(new EndpointHostConfig { DebugMode = true });
			container.Register<IRedisClientsManager>(c => new PooledRedisClientManager());
			container.Register<IRepository>(c => new Repository(c.Resolve<IRedisClientsManager>()));
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