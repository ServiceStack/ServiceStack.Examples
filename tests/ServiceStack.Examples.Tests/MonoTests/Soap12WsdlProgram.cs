using System;
using ServiceStack.Examples.ServiceModel;
using ServiceStack.Metadata;

namespace ServiceStack.Examples.Tests.MonoTests
{
	public class Soap12WsdlProgram
	{
		public static void Main()
		{
			var xsd = new XsdGenerator
			{
				OperationTypes = new[] {
					typeof(GetUsers), typeof(DeleteAllUsers), typeof(StoreNewUser), 
					typeof(GetFactorial), typeof(GetFibonacciNumbers)
				},
				OptimizeForFlash = false,				
			}.ToString();

			Console.WriteLine("xsd: " + xsd);
		}
	}
}