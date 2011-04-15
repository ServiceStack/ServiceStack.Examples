using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace ServiceStack.Examples.ServiceModel.Operations
{
	/// <summary>
	/// Use Plain old DataContract's Define your 'Service Interface'
	/// 
	/// This purpose of this example is how you would implement a more advanced
	/// web service returning a slightly more 'complex object'.
	/// </summary>
	public class GetFibonacciNumbers
	{
		public long? Skip { get; set; }

		public long? Take { get; set; }
	}

	public class GetFibonacciNumbersResponse
	{
		public ArrayOfLong Results { get; set; }
	}
}