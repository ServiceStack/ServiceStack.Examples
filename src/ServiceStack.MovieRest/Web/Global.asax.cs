namespace ServiceStack.MovieRest
{
	using System;
	using System.Web;
	using Funq;
	using ServiceStack.Common.Utils;
	using ServiceStack.OrmLite;
	using ServiceStack.OrmLite.Sqlite;
	using ServiceStack.ServiceInterface.Cors;
	using ServiceStack.WebHost.Endpoints;

	/// <summary>
	///     Create your ServiceStack web service application with a singleton AppHost.
	/// </summary>
	public class MovieAppHost : AppHostBase
	{
		/// <summary>
		///     Initializes a new instance of your ServiceStack application, with the specified name and assembly containing the services.
		/// </summary>
		public MovieAppHost() : base("ServiceStack REST at the Movies!", typeof (MovieService).Assembly)
		{
		}

		/// <summary>
		///     Configure the container with the necessary routes for your ServiceStack application.
		/// </summary>
		/// <param name="container">The built-in IoC used with ServiceStack.</param>
		public override void Configure(Container container)
		{
			//JsConfig.DateHandler = JsonDateHandler.ISO8601;

			container.Register<IDbConnectionFactory>(c => new OrmLiteConnectionFactory("~/App_Data/db.sqlite".MapHostAbsolutePath(), SqliteOrmLiteDialectProvider.Instance));

			//Call existing service
			using (var resetMovies = container.Resolve<ResetMoviesService>())
			{
				resetMovies.Any(null);
			}

			Plugins.Add(new CorsFeature()); //Enable CORS

			SetConfig(new EndpointHostConfig
				          {
					          DebugMode = true, //Show StackTraces for easier debugging (default auto inferred by Debug/Release builds)
				          });
		}
	}

	public class Global : HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			//Initialize your application
			(new MovieAppHost()).Init();
		}
	}
}