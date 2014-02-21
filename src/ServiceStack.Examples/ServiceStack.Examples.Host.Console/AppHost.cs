using Funq;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.Examples.ServiceInterface;
using ServiceStack.Examples.ServiceInterface.Support;
using ServiceStack.Examples.ServiceModel;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;
using ExampleConfig = ServiceStack.Examples.ServiceInterface.ExampleConfig;

namespace ServiceStack.Examples.Host.Console
{
	/// <summary>
	/// An example of a AppHost to have your services running inside a webserver.
	/// </summary>
	public class AppHost
		: AppHostHttpListenerBase
	{
		private static ILog log;

		public AppHost()
			: base("ServiceStack Examples", typeof(GetFactorialService).Assembly)
		{
			LogManager.LogFactory = new DebugLogFactory();
			log = LogManager.GetLogger(typeof(AppHost));
		}

		public override void Configure(Container container)
		{
			//Signal advanced web browsers what HTTP Methods you accept
			base.SetConfig(new HostConfig
			{
                DebugMode = true,
			});

            container.Register<IAppSettings>(new AppSettings());

            var appSettings = container.Resolve<IAppSettings>();

            container.Register(c => new ExampleConfig(c.Resolve<IAppSettings>()));
			var appConfig = container.Resolve<ExampleConfig>();

			container.Register<IDbConnectionFactory>(c => 
				new OrmLiteConnectionFactory(
					appConfig.ConnectionString.MapAbsolutePath(), 
					SqliteOrmLiteDialectProvider.Instance));

			if (appSettings.Get("PerformTestsOnInit", false))
			{
				log.Debug("Performing database tests...");
				DatabaseTest(container.Resolve<IDbConnectionFactory>());
			}
		}

		private void DatabaseTest(IDbConnectionFactory connectionFactory)
		{
			ConfigureDatabase.Init(connectionFactory);

			var storeRequest = new StoreNewUser {
				Email = "new@email",
				Password = "password",
				UserName = "new UserName"
			};

            var storeHandler = Container.Resolve<StoreNewUserService>();
			storeHandler.Any(storeRequest);

            var getAllHandler = Container.Resolve<GetAllUsersService>();
            var response = (GetAllUsersResponse)getAllHandler.Any(new GetAllUsers());

			var user = response.Users[0];

			System.Console.WriteLine("Stored and retrieved user: {0}, {1}, {2}",
				user.Id, user.UserName, user.Email);
		}

	}
}