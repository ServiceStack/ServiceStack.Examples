using System;
using Funq;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.Common.Utils;
using Northwind.ServiceInterface;
using ServiceStack.OrmLite;
using ServiceStack.WebHost.Endpoints;

namespace Northwind
{
    /// <summary>
    /// Create your ServiceStack web service application with a singleton AppHost.
    /// </summary>
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Initializes a new instance of your ServiceStack application, with the specified name and assembly containing the services.
        /// </summary>
        public AppHost()
            : base("Northwind Web Services", typeof(CustomersService).Assembly) {}

        /// <summary>
        /// Configure the container with the necessary routes for your ServiceStack application.
        /// </summary>
        /// <param name="container">The built-in IoC used with ServiceStack.</param>
        public override void Configure(Container container)
        {
            container.Register<IDbConnectionFactory>(
                new OrmLiteConnectionFactory("~/Nortwind.sqlite".MapHostAbsolutePath(), SqliteDialect.Provider));

            //Using an in-memory cache
            container.Register<ICacheClient>(new MemoryCacheClient());

            //Or if Haz Redis
            //container.Register<ICacheClient>(new PooledRedisClientManager());

            VCardFormat.Register(this);
        }
    }

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //Initialize your application
            (new AppHost()).Init();
        }
    }
}