using ServiceStack.Examples.ServiceModel;

namespace ServiceStack.Examples.ServiceInterface
{
	/// <summary>
	/// An example of a very basic web service
	/// </summary>
	public class GreetService : Service
	{
		public object Any(Greet request)
		{
			return new GreetResponse { Result = "Hello " + request.Name };
		}
	}
}