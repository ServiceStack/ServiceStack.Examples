using System;
using System.ServiceProcess;
using System.Threading;
using StarterTemplates.Common;

namespace WinServiceAppHost
{
	static class Program
	{
		private const string ListeningOn = "http://localhost:83/";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			var appHost = new StarterTemplateAppListenerHost();

			//Allow you to debug your Windows Service while you're deleloping it. 
#if DEBUG
			Console.WriteLine("Running WinServiceAppHost in Console mode");
			try
			{
				appHost.Init();
				appHost.Start(ListeningOn);

				Console.WriteLine("Press <CTRL>+C to stop.");
				Thread.Sleep(Timeout.Infinite);
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR: {0}: {1}", ex.GetType().Name, ex.Message);
				throw;
			}
			finally
			{
				appHost.Stop();
			}

			Console.WriteLine("WinServiceAppHost has finished");

#else
			//When in RELEASE mode it will run as a Windows Service with the code below

			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[] 
			{ 
				new WinService(appHost, ListeningOn) 
			};
			ServiceBase.Run(ServicesToRun);
#endif

		}
	}
}
