using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;
using CustomerOrders=ServiceStack.Examples.ServiceModel.Types.CustomerOrders;

namespace ServiceStack.Examples.ServiceModel.Operations
{
	public class GetNorthwindCustomerOrders
	{
		public string CustomerId { get; set; }
	}

	public class GetNorthwindCustomerOrdersResponse
	{
		public GetNorthwindCustomerOrdersResponse()
		{
			this.ResponseStatus = new ResponseStatus();
		}

		public CustomerOrders CustomerOrders { get; set; }

		public ResponseStatus ResponseStatus { get; set; }
	}
}