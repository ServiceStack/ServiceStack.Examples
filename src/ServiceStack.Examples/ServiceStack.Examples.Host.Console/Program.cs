using System;
using System.Text;
using System.Threading;

namespace ServiceStack.Examples.Host.Console
{
	class Program
	{
		private const string ListeningOn = "http://localhost:82/";

		static void Main(string[] args)
		{
			var appHost = new AppHost();
			appHost.Init();
			appHost.Start(ListeningOn);

			System.Console.WriteLine("AppHost Created at {0}, listening on {1}",
				DateTime.Now, ListeningOn);

			var sb = new StringBuilder();
			sb.AppendLine("Some urls for you to try:\n");
			sb.AppendLine(ListeningOn + "xml/syncreply/GetFactorial?ForNumber=5");
			sb.AppendLine(ListeningOn + "json/syncreply/GetFibonacciNumbers?Skip=5&Take=10");
			sb.AppendLine(ListeningOn + "jsv/syncreply/GetAllUsers?debug");

			System.Console.WriteLine(sb);
			

			Thread.Sleep(Timeout.Infinite);
			System.Console.WriteLine("ReadLine()");
			System.Console.ReadLine();
		}
	}
}
 