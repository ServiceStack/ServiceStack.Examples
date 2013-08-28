namespace ServiceStack.Northwind.ServiceModel.Operations
{
	using System.Runtime.Serialization;
	using ServiceStack.ServiceHost;

	[DataContract]
	[Route("/cached/customers")]
	public class CachedCustomers
	{
	}

	[DataContract]
	[Route("/cached/customers/{Id}")]
	public class CachedCustomerDetails
	{
		[DataMember]
		public string Id { get; set; }
	}

	[DataContract]
	[Route("/cached/orders")]
	[Route("/cached/orders/page/{Page}")]
	[Route("/cached/customers/{CustomerId}/orders")]
	public class CachedOrders
	{
		[DataMember]
		public int? Page { get; set; }

		[DataMember]
		public string CustomerId { get; set; }
	}
}