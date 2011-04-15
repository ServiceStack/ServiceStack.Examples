using System.Runtime.Serialization;

namespace ServiceStack.Examples.ServiceModel.Operations
{
	/// <summary>
	/// Use Plain old DataContract's Define your 'Service Interface'.
	/// 
	/// The purpose of this example is to show the minimum number and detail of classes 
	/// required in order to implement a simple service.
	/// </summary>
	public class GetFactorial
	{
		public long ForNumber { get; set; }
	}

	public class GetFactorialResponse
	{
		public long Result { get; set; }
	}
}