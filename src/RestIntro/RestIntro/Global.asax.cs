using System;
using Funq;
using RestIntro.ServiceInterface;
using RestIntro.ServiceModel;
using ServiceStack.Common.Utils;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;
using ServiceStack.WebHost.Endpoints;

namespace RestIntro
{
	public class AppHost : AppHostBase
	{
		public AppHost() : base("REST Intro", typeof(CustomerService).Assembly) { }

		public override void Configure(Container container)
		{
			container.Register<IDbConnectionFactory>(
				new OrmLiteConnectionFactory(
				"~/RestIntro.sqlite".MapHostAbsolutePath(),
				SqliteOrmLiteDialectProvider.Instance));

			container.Resolve<IDbConnectionFactory>()
				.Exec(dbCmd => dbCmd.CreateTable<Customer>(true));
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