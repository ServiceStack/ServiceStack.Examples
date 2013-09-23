namespace ServiceStack.Northwind
{
	using System;
	using System.Web;
	using Funq;
	using CacheAccess;
	using CacheAccess.Providers;
	using Common.Utils;
	using ServiceInterface;
	using OrmLite;
	using WebHost.Endpoints;

	public class AppHost : AppHostBase
	{
		public AppHost() : base("Northwind Web Services", typeof(CustomersService).Assembly) {}

		public override void Configure(Container container)
		{
			container.Register<IDbConnectionFactory>(
				new OrmLiteConnectionFactory("~/Northwind.sqlite".MapHostAbsolutePath(), SqliteDialect.Provider));

			//Using an in-memory cache
			container.Register<ICacheClient>(new MemoryCacheClient());

			//Or if Haz Redis
			//container.Register<ICacheClient>(new PooledRedisClientManager());

			VCardFormat.Register(this);
		}
	}

	public class Global : HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			new AppHost().Init();
		}
	}
}