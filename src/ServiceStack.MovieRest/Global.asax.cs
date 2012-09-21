using System;
using Funq;
using ServiceStack.Common.Utils;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;

namespace ServiceStack.MovieRest
{
    /// <summary>
    /// Create your ServiceStack web service application with a singleton AppHost.
    /// </summary>  
    public class MovieAppHost : AppHostBase
    {
        /// <summary>
        /// Initializes a new instance of your ServiceStack application, with the specified name and assembly containing the services.
        /// </summary>
        public MovieAppHost() : base("ServiceStack REST at the Movies!", typeof(MovieService).Assembly) { }

        /// <summary>
        /// Configure the container with the necessary routes for your ServiceStack application.
        /// </summary>
        /// <param name="container">The built-in IoC used with ServiceStack.</param>
        public override void Configure(Container container)
        {
            JsConfig.DateHandler = JsonDateHandler.ISO8601;

            container.Register<IDbConnectionFactory>(c =>
                new OrmLiteConnectionFactory(
                    "~/App_Data/db.sqlite".MapHostAbsolutePath(),
                    SqliteOrmLiteDialectProvider.Instance));

            var resetMovies = container.Resolve<ResetMoviesService>();
            resetMovies.Post(null);

            Routes
              .Add<Movie>("/movies")
              .Add<Movie>("/movies/{Id}")
              .Add<Movies>("/movies")
              .Add<Movies>("/movies/genres/{Genre}");

            SetConfig(new EndpointHostConfig
            {
                GlobalResponseHeaders = {
                        { "Access-Control-Allow-Origin", "*" },
                        { "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS" },
                        { "Access-Control-Allow-Headers", "Content-Type, X-Requested-With" },
                    },
                //EnableFeatures = onlyEnableFeatures,
                //DebugMode = true, //Show StackTraces for easier debugging
            });

        }
    }

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //Initialize your application
            (new MovieAppHost()).Init();
        }
    }
}