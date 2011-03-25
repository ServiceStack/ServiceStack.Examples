using System.ComponentModel;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

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
}