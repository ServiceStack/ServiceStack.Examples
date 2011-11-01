using System.Collections.Generic;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace ServiceStack.Northwind.ServiceModel.Operations
{
	[RestService("/orders")]
	[RestService("/orders/page/{Page}")]
	[RestService("/customers/{CustomerId}/orders")]
	public class Orders
	{
		public int? Page { get; set; }

		public string CustomerId { get; set; }
	}

	public class OrdersResponse : IHasResponseStatus
	{
		public OrdersResponse()
		{
			this.ResponseStatus = new ResponseStatus();
			this.Results = new List<CustomerOrder>();
		}

		public List<CustomerOrder> Results { get; set; }

		public ResponseStatus ResponseStatus { get; set; }
	}

}
