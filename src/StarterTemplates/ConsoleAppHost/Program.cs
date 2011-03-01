using System;
using System.Threading;
using ServiceStack.WebHost.Endpoints;
using StarterTemplates.Common;

namespace ConsoleAppHost
{
	class Program
	{
		private const string ListeningOn = "http://localhost:82/";

		static void Main(string[] args)
		{
			var appHost = new StarterTemplateAppListenerHost(ListeningOn, "ConsoleAppHost");
			appHost.Init();

			Console.WriteLine("AppHost Created at {0}, listening on {1}",
				DateTime.Now, ListeningOn);


			Thread.Sleep(Timeout.Infinite);
			Console.WriteLine("ReadKey()");
			Console.ReadKey();
		}
	}
}
