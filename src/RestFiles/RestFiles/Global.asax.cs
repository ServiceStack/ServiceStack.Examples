using System;
using System.IO;
using Funq;
using RestFiles.ServiceInterface;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.Logging.Support.Logging;
using ServiceStack.WebHost.Endpoints;

namespace RestFiles
{
	/// <summary>
	/// The Singleton AppHost class. 
	/// Set initial ServiceStack options and register your web services dependencies and run onload scripts
	/// </summary>
	public class AppHost
		: AppHostBase
	{
		public AppHost()
			: base("REST Files", typeof(FilesService).Assembly) {}

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

			var config = new AppConfig(new ConfigurationResourceManager());
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
			var appHost = new AppHost();
			appHost.Init();
		}
	}
}