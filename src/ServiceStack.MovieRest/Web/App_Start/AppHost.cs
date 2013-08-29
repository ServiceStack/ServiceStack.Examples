using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using ServiceStack.Configuration;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.WebHost.Endpoints;

[assembly: WebActivator.PreApplicationStartMethod(typeof(ServiceStack.MovieRest.App_Start.MovieAppHost), "Start")]


/**
 * Entire ServiceStack Starter Template configured with a 'Hello' Web Service and a 'Todo' Rest Service.
 *
 * Auto-Generated Metadata API page at: /metadata
 * See other complete web service examples at: https://github.com/ServiceStack/ServiceStack.Examples
 */

namespace ServiceStack.MovieRest.App_Start
{
	using ServiceStack.Common.Utils;
	using ServiceStack.OrmLite.Sqlite;
	using ServiceStack.ServiceInterface.Cors;

	public class MovieAppHost
		: AppHostBase
	{		
		/// <summary>
		///     Initializes a new instance of your ServiceStack application, with the specified name and assembly containing the services.
		/// </summary>
		public MovieAppHost() : base("ServiceStack REST at the Movies!", typeof (MovieService).Assembly)
		{
		}

		public override void Configure(Funq.Container container)
		{
			//Set JSON web services to return idiomatic JSON camelCase properties
			ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

			container.Register<IDbConnectionFactory>(c => new OrmLiteConnectionFactory("~/App_Data/db.sqlite".MapHostAbsolutePath(), SqliteOrmLiteDialectProvider.Instance));

			using (var resetMovies = container.Resolve<ResetMoviesService>())
			{
				resetMovies.Any(null);
			}

			Plugins.Add(new CorsFeature()); //Enable CORS

			SetConfig(new EndpointHostConfig
			{
				DebugMode = true //Show StackTraces for easier debugging (default auto inferred by Debug/Release builds)
			});
		}

		

		public static void Start()
		{
			new MovieAppHost().Init();
		}
	}
}
