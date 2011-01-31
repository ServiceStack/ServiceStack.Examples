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
	/// Web Services with http://www.servicestack.net/
	/// 
	/// Configuring ServiceStack to run inside a webserver.
	/// </summary>
	public class AppHost 
		: AppHostBase
	{
		private static ILog log;

		public AppHost() 
			: base("MonoTouch RemoteInfo", typeof(FilesService).Assembly)
		{
			LogManager.LogFactory = new DebugLogFactory();
			log = LogManager.GetLogger(GetType());
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
			
			var config = new AppConfig(new ConfigurationResourceManager());
			container.Register(config);

			if (!Directory.Exists(config.RootDirectory))
			{
				Directory.CreateDirectory(config.RootDirectory);
			}

			log.InfoFormat("AppHost Configured: " + DateTime.Now);
		}
	}
}