using System;
using Funq;
using RedisStackOverflow.ServiceInterface;
using ServiceStack.Redis;
using ServiceStack.WebHost.Endpoints;

namespace RedisStackOverflow
{
	public class AppHost
		: AppHostBase
	{
		public AppHost()
			: base("Redis StackOverflow", typeof(QuestionsService).Assembly) { }

		public override void Configure(Container container)
		{
		    //Show StackTrace in Web Service Exceptions
			SetConfig(new EndpointHostConfig { DebugMode = true });
			
			//Register any dependencies you want injected into your services
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