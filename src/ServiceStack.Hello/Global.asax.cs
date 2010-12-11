using System;
using System.Runtime.Serialization;
using Funq;
using ServiceStack.ServiceHost;
using ServiceStack.WebHost.Endpoints;

namespace ServiceStack.Hello
{
	/// Create the name of your Web Service (i.e. the Request DTO)
	[DataContract]
	[RestService("/hello/{Name}")] //Optional: Define an alternate REST-ful url for this service
	public class Hello
	{
		[DataMember]
		public string Name { get; set; }
	}

	/// Define your Web Service response (i.e. Response DTO)
	[DataContract]
	public class HelloResponse
	{
		[DataMember]
		public string Result { get; set; }
	}

	/// Create your Web Service implementation 
	public class HelloService : IService<Hello>
	{
		public object Execute(Hello request)
		{
			return new HelloResponse { Result = "Hello, " + request.Name };
		}
	}


	public class Global : System.Web.HttpApplication
	{
		/// Web Service Singleton AppHost
		public class HelloAppHost : AppHostBase
		{
			//Tell Service Stack the name of your application and where to find your web services
			public HelloAppHost() 
				: base("Hello Web Services", typeof(HelloService).Assembly) { }
			
			public override void Configure(Container container) { }
		}

		protected void Application_Start(object sender, EventArgs e)
		{
			//Initialize your application
			var appHost = new HelloAppHost();
			appHost.Init();
		}
	}


}