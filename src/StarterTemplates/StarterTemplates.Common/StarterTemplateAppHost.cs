using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using ServiceStack.Configuration;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.WebHost.Endpoints;

namespace StarterTemplates.Common
{
	[DataContract]
	[Description("ServiceStack's Hello World web service.")]
	[RestService("/hello")]
	[RestService("/hello/{Name*}")]
	public class Hello
	{
		[DataMember]
		public string Name { get; set; }
	}

	[DataContract]
	public class HelloResponse : IHasResponseStatus
	{
		[DataMember]
		public string Result { get; set; }

		[DataMember]
		public ResponseStatus ResponseStatus { get; set; }
	}

	public class HelloService : ServiceBase<Hello>
	{
		protected override object Run(Hello request)
		{
			return new HelloResponse { Result = "Hello, " + request.Name };
		}
	}

	//ASP.NET Hosts
	public class StarterTemplateAppHost
		: AppHostBase
	{
		static readonly ConfigurationResourceManager AppSettings = new ConfigurationResourceManager();

		public StarterTemplateAppHost()
			: base(AppSettings.GetString("ServiceName") ?? "StarterTemplate ASP.NET Host", typeof(HelloService).Assembly)
		{
		}

		public override void Configure(Funq.Container container) { }
	}

	//HttpListener Hosts
	public class StarterTemplateAppListenerHost
		: AppHostHttpListenerBase
	{
		static readonly ConfigurationResourceManager AppSettings = new ConfigurationResourceManager();

		private readonly string listentingOn;

		public StarterTemplateAppListenerHost(string listentingOn, string handlerPath)
			: base(AppSettings.GetString("ServiceName") ?? "StarterTemplate HttpListener", typeof(HelloService).Assembly)
		{
			this.listentingOn = listentingOn;
			EndpointHostConfig.ServiceStackPath = handlerPath;
		}

		public override void Configure(Funq.Container container)
		{
			this.Start(this.listentingOn);
			Console.WriteLine("Started listening on: " + listentingOn);
		}
	}
}
