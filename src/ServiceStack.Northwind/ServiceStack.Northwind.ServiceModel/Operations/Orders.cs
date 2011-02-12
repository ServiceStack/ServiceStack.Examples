using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace ServiceStack.Northwind.ServiceModel.Operations
{
	[DataContract]
	[RestService("/orders")]
	[RestService("/orders/page/{Page}")]
	[RestService("/customers/{CustomerId}/orders")]
	public class Orders
	{
		[DataMember]
		public int? Page { get; set; }

		[DataMember]
		public string CustomerId { get; set; }
	}

	[DataContract]
	public class OrdersResponse : IHasResponseStatus
	{
		public OrdersResponse()
		{
			this.ResponseStatus = new ResponseStatus();
			this.Results = new List<CustomerOrder>();
		}

		[DataMember]
		public List<CustomerOrder> Results { get; set; }

		[DataMember]
		public ResponseStatus ResponseStatus { get; set; }
	}

}