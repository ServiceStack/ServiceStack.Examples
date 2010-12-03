using System;
using NUnit.Framework;
using ServiceStack.Examples.ServiceModel.Types;
using ServiceStack.Text;

namespace ServiceStack.Examples.Tests
{
	[TestFixture]
	public class LoggerTests
	{
		[Test]
		public void Create_test_data()
		{
			var loggers = new ArrayOfLogger
			{
				new Logger
				{
					Id = 786,
					Devices = new ArrayOfDevice
					{
						new Device
                      	{
                      		Id = 5955,
							Type = "Panel",
							TimeStamp = 1199303309,
							Channels = new ArrayOfChannel
				           	{
				           		{new Channel("Temperature", "58")},
				           		{new Channel("Status", "On")},
				           	}
                      	},
						new Device
                      	{
                      		Id = 5956,
							Type = "Tank",
							TimeStamp = 1199303309,
							Channels = new ArrayOfChannel
				           	{
				           		{ new Channel("Volume", "10035") },
				           		{ new Channel("Status", "Full") },
				           	}
                      	},
					}
				}
			};

			var jsv = TypeSerializer.SerializeToString(loggers);
			Console.WriteLine(jsv);
		}

	}
}