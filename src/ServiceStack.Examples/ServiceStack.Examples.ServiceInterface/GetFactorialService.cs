using ServiceStack.Examples.ServiceModel;

namespace ServiceStack.Examples.ServiceInterface
{
	/// <summary>
	/// The purpose of this example is to show the minimum number and detail of classes 
	/// required in order to implement a simple service.
	/// </summary>
	public class GetFactorialService : Service
	{
        public GetFactorialResponse Any(GetFactorial request)
		{
			return new GetFactorialResponse { Result = GetFactorial(request.ForNumber) };
		}

		static long GetFactorial(long n)
		{
			return n > 1 ? n * GetFactorial(n - 1) : 1;
		}
	}
}