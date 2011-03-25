using System;
using ServiceStack.Configuration;
using ServiceStack.WebHost.Endpoints;
using StarterTemplates.Common;

namespace ConsoleAppHost
{
	class Program
	{
		private static readonly string ListeningOn = ConfigUtils.GetAppSetting("ListeningOn");

		//HttpListener Hosts
		public class AppHost
			: AppHostHttpListenerBase
		{
			public AppHost()
				: base("StarterTemplate HttpListener", typeof(HelloService).Assembly) { }

			public override void Configure(Funq.Container container)
			{
				container.Register(new TodoRepository());
			}
		}

		static void Main(string[] args)
		{
			var appHost = new AppHost();
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
