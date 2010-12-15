using System;
using Funq;
using ServiceStack.Common.Utils;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;
using ServiceStack.WebHost.Endpoints;

namespace ServiceStack.MovieRest
{
	public class AppHost
		: AppHostBase
	{
		public AppHost()
			: base("ServiceStack REST at the Movies!", typeof(MovieService).Assembly) {}

		public override void Configure(Container container)
		{
			container.Register<IDbConnectionFactory>(c =>
				new OrmLiteConnectionFactory(
					"~/App_Data/db.sqlite".MapHostAbsolutePath(),
					SqliteOrmLiteDialectProvider.Instance));

			var moviesService = container.Resolve<ResetMoviesService>();
			moviesService.Post(null);
		}
	}

	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			new AppHost().Init();
		}

		protected void Session_Start(object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{

		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{

		}
	}
}