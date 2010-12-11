using System;
using System.Runtime.Serialization;
using Funq;
using ServiceStack.ServiceHost;
using ServiceStack.WebHost.Endpoints;

namespace ServiceStack.Hello
{
	[DataContract]
	[RestService("/hello/{Name}")]
	public class Hello
	{
		[DataMember]
		public string Name { get; set; }
	}

	[DataContract]
	public class HelloResponse
	{
		[DataMember]
		public string Result { get; set; }
	}

	public class HelloService : IService<Hello>
	{
		public object Execute(Hello request)
		{
			return new HelloResponse { Result = "Hello, " + request.Name };
		}
	}

	public class Global : System.Web.HttpApplication
	{
		public class HelloAppHost : AppHostBase
		{
			public HelloAppHost() : base("Hello Web Services", typeof(HelloService).Assembly) { }
			public override void Configure(Container container) { }
		}

		protected void Application_Start(object sender, EventArgs e)
		{
			var appHost = new HelloAppHost();
			appHost.Init();
		}
	}
}