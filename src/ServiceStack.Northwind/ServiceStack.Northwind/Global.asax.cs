using System;
using Funq;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.Common.Utils;
using ServiceStack.Northwind.ServiceInterface;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;
using ServiceStack.Redis;
using ServiceStack.WebHost.Endpoints;

namespace ServiceStack.Northwind
{
	public class AppHost 
		: AppHostBase
	{
		public AppHost() : base("Northwind Web Services", typeof(CustomersService).Assembly) { }

		public override void Configure(Container container)
		{
			container.Register<IDbConnectionFactory>(
				new OrmLiteConnectionFactory(
				"~/Nortwind.sqlite".MapHostAbsolutePath(), 
				SqliteOrmLiteDialectProvider.Instance));

			//Using an in-memory cache
			container.Register<ICacheClient>(new MemoryCacheClient());

			//Or if Haz Redis
			//container.Register<ICacheClient>(new PooledRedisClientManager());
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