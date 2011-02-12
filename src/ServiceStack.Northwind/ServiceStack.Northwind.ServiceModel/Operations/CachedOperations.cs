using System.Runtime.Serialization;
using ServiceStack.ServiceHost;

namespace ServiceStack.Northwind.ServiceModel.Operations
{
	[DataContract]
	[RestService("/cached/customers")]
	public class CachedCustomers {}

	[DataContract]
	[RestService("/cached/customers/{Id}")]
	public class CachedCustomerDetails
	{
		[DataMember]
		public string Id { get; set; }
	}

	[DataContract]
	[RestService("/cached/orders")]
	[RestService("/cached/orders/page/{Page}")]
	[RestService("/cached/customers/{CustomerId}/orders")]
	public class CachedOrders
	{
		[DataMember]
		public int? Page { get; set; }

		[DataMember]
		public string CustomerId { get; set; }
	}

}