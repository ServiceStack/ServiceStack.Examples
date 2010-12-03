using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ServiceStack.Examples.ServiceModel.Types
{
	[DataContract(Namespace = ExampleConfig.DefaultNamespace)]
	public class Logger
	{
		public Logger()
		{
			this.Devices = new ArrayOfDevice();
		}

		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public ArrayOfDevice Devices { get; set; }
	}

	[CollectionDataContract(Namespace = ExampleConfig.DefaultNamespace, ItemName = "Logger")]
	public class ArrayOfLogger : List<Logger>
	{
		public ArrayOfLogger() { }
		public ArrayOfLogger(IEnumerable<Logger> collection) : base(collection) { }
	}

	[DataContract(Namespace = ExampleConfig.DefaultNamespace)]
	public class Device 
	{
		public Device()
		{
			this.Channels = new ArrayOfChannel();
		}

		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public string Type { get; set; }

		[DataMember]
		public long TimeStamp { get; set; }

		[DataMember]
		public ArrayOfChannel Channels { get; set; }
	}

	[CollectionDataContract(Namespace = ExampleConfig.DefaultNamespace, ItemName = "Device")]
	public class ArrayOfDevice : List<Device>
	{
		public ArrayOfDevice() { }
		public ArrayOfDevice(IEnumerable<Device> collection) : base(collection) { }
	}

	[DataContract(Namespace = ExampleConfig.DefaultNamespace)]
	public class Channel
	{
		public Channel()
		{
		}

		public Channel(string name, string value)
		{
			Name = name;
			Value = value;
		}

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Value { get; set; }
	}

	[CollectionDataContract(Namespace = ExampleConfig.DefaultNamespace, ItemName = "Channel")]
	public class ArrayOfChannel : List<Channel>
	{
		public ArrayOfChannel() { }
		public ArrayOfChannel(IEnumerable<Channel> collection) : base(collection) { }
	}
}
