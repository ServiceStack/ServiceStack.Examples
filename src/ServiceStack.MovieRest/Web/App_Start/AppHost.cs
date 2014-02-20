
namespace ServiceStack.MovieRest.App_Start
{
	using Funq;
	using OrmLite;
	using Text;
    using Data;

    public class AppHost
		: AppHostBase
	{
		/// <summary>
		///     Initializes a new instance of your ServiceStack application, with the specified name and assembly containing the services.
		/// </summary>
		public AppHost() : base("ServiceStack REST at the Movies!", typeof (MovieService).Assembly) {}

		public override void Configure(Container container)
		{
			JsConfig.DateHandler = DateHandler.ISO8601;

			//Set JSON web services to return idiomatic JSON camelCase properties
			JsConfig.EmitCamelCaseNames = true;

			container.Register<IDbConnectionFactory>(
				c => new OrmLiteConnectionFactory("~/App_Data/db.sqlite".MapHostAbsolutePath(), SqliteDialect.Provider));

			using (var resetMovies = container.Resolve<ResetMoviesService>())
			{
				resetMovies.Any(null);
			}

			Plugins.Add(new CorsFeature()); //Enable CORS

			SetConfig(new HostConfig {
				DebugMode = true //Show StackTraces for easier debugging (default auto inferred by Debug/Release builds)
			});
		}
	}
}