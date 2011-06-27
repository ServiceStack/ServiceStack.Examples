using ServiceStack.ServiceInterface;

namespace PhotoGallery.Logic
{
	public class Hello
	{
		public string Name { get; set; }
	}

	public class HelloResponse
	{
		public string Result { get; set; }
	}

	public class HelloService : RestServiceBase<Hello>
	{
		public override object OnGet(Hello request)
		{
			return new HelloResponse { Result = "Hello, " + request.Name };
		}
	}
}
