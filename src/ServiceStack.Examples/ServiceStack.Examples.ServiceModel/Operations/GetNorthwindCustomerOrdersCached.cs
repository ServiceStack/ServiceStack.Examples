using System;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;
using CustomerOrders=ServiceStack.Examples.ServiceModel.Types.CustomerOrders;

namespace ServiceStack.Examples.ServiceModel.Operations
{
	public class GetNorthwindCustomerOrdersCached
	{
		public bool RefreshCache { get; set; }

		public string CustomerId { get; set; }
	}

	public class GetNorthwindCustomerOrdersCachedResponse
	{
		public GetNorthwindCustomerOrdersCachedResponse()
		{
			this.ResponseStatus = new ResponseStatus();
		}

		public DateTime CreatedDate { get; set; }

		public CustomerOrders CustomerOrders { get; set; }

		public ResponseStatus ResponseStatus { get; set; }
	}
}