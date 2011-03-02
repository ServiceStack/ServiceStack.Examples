using System;
using System.Threading;
using StarterTemplates.Common;

namespace ConsoleAppHost
{
	class Program
	{
		private const string ListeningOn = "http://localhost:82/";

		static void Main(string[] args)
		{
			var appHost = new StarterTemplateAppListenerHost();
			appHost.Init();
			appHost.Start(ListeningOn);

			Console.WriteLine("Started listening on: " + ListeningOn);

			Console.WriteLine("AppHost Created at {0}, listening on {1}",
				DateTime.Now, ListeningOn);


			Console.WriteLine("ReadKey()");
			Console.ReadKey();
		}
	}
}
