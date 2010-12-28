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
			: base("ServiceStack Questions", typeof(QuestionsService).Assembly)
		{
		}

		public override void Configure(Container container)
		{
			container.Register<IRedisClientsManager>(c => new PooledRedisClientManager());
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