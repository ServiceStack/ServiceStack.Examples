using System;
using System.IO;
using Funq;
using RestFiles.ServiceInterface;
using ServiceStack;
using ServiceStack.Configuration;

namespace RestFiles
{
    /// <summary>
    /// Create your ServiceStack web service application with a singleton AppHost.
    /// </summary> 
    public class AppHost : AppHostBase  
    {
        /// <summary>
        /// Initializes a new instance of your ServiceStack application, with the specified name and assembly containing the services.
        /// </summary>
        public AppHost() : base("REST Files", typeof(FilesService).Assembly) {}

        /// <summary>
        /// Configure the container with the necessary routes for your ServiceStack application.
        /// </summary>
        /// <param name="container">The built-in IoC used with ServiceStack.</param>
        public override void Configure(Container container)
        {
            //Permit modern browsers (e.g. Firefox) to allow sending of any REST HTTP Method
            Plugins.Add(new CorsFeature());

            SetConfig(new HostConfig {
                DebugMode = true,
            });

            var config = new AppConfig(new AppSettings());
            container.Register(config);

            if (!Directory.Exists(config.RootDirectory))
            {
                Directory.CreateDirectory(config.RootDirectory);
            }
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