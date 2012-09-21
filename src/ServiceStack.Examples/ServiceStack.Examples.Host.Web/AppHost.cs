using System;
using Funq;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.Common.Utils;
using ServiceStack.Configuration;
using ServiceStack.DataAccess;
using ServiceStack.Examples.ServiceInterface;
using ServiceStack.Examples.ServiceInterface.Support;
using ServiceStack.Logging;
using ServiceStack.Logging.Support.Logging;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;
using ServiceStack.Redis;
using ServiceStack.WebHost.Endpoints;

namespace ServiceStack.Examples.Host.Web
{
    /// <summary>
    /// An example of a AppHost to have your services running inside a webserver.
    /// </summary>
    public class AppHost : AppHostBase
    {
        private static ILog log;

        public AppHost() : base("ServiceStack Examples", typeof(GetFactorialService).Assembly)
        {
            LogManager.LogFactory = new ConsoleLogFactory();
            log = LogManager.GetLogger(typeof (AppHost));
        }

        public override void Configure(Container container)
        {
            //Permit modern browsers (e.g. Firefox) to allow sending of any REST HTTP Method
            base.SetConfig(new EndpointHostConfig
            {
                GlobalResponseHeaders =
                    {
                        { "Access-Control-Allow-Origin", "*" },
                        { "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS" },
                    },
            });

            container.Register<IResourceManager>(new ConfigurationResourceManager());
            container.Register(c => new ExampleConfig(c.Resolve<IResourceManager>()));

            var appConfig = container.Resolve<ExampleConfig>();

            container.Register<IDbConnectionFactory>(c =>
                new OrmLiteConnectionFactory(
                    appConfig.ConnectionString.MapHostAbsolutePath(),
                    SqliteOrmLiteDialectProvider.Instance));

            ConfigureDatabase.Init(container.Resolve<IDbConnectionFactory>());

            //The MemoryCacheClient is a great way to get started with caching; nothing external to setup.
            container.Register<ICacheClient>(c => new MemoryCacheClient());

            //If you give Redis a try, you won't be disappointed. This however requires Redis to be installed.
            //container.Register<ICacheClient>(c => new BasicRedisClientManager());

            log.InfoFormat("AppHost Configured: {0}", DateTime.Now);
        }
    }

}