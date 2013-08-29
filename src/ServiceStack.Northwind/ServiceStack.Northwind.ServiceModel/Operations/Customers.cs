namespace ServiceStack.Northwind.ServiceModel.Operations
{
	using System.Collections.Generic;
	using System.Runtime.Serialization;
	using ServiceStack.Northwind.ServiceModel.Types;
	using ServiceStack.ServiceHost;
	using ServiceStack.ServiceInterface.ServiceModel;

	[DataContract]
	[Route("/customers")]
	public class Customers
	{
	}

	[DataContract]
	public class CustomersResponse : IHasResponseStatus
	{
		public CustomersResponse()
		{
			this.ResponseStatus = new ResponseStatus();
			this.Customers = new List<Customer>();
		}

		[DataMember]
		public List<Customer> Customers { get; set; }

		[DataMember]
		public ResponseStatus ResponseStatus { get; set; }
	}
}