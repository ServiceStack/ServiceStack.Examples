using System;
using ServiceStack.WebHost.Endpoints;
using StarterTemplates.Common;
using Funq;

namespace CustomPath40
{
    /// <summary>
    /// Create your ServiceStack web service application with a singleton AppHost.
    /// </summary> 
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Initializes a new instance of your ServiceStack application, with the specified name and assembly containing the services.
        /// </summary>
        public AppHost() : base("StarterTemplate ASP.NET Host", typeof(HelloService).Assembly) { }

        /// <summary>
        /// Configure the container with the necessary routes for your ServiceStack application.
        /// </summary>
        /// <param name="container">The built-in IoC used with ServiceStack.</param>
        public override void Configure(Container container)
        {
            container.Register(new TodoRepository());
        }
    }

    public class Global : System.Web.HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            //Initialize your application
            (new AppHost()).Init();
        }
    }
}
