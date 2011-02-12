using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace ServiceStack.Northwind.ServiceModel.Operations
{
	[DataContract]
	[RestService("/customers/{Id}")]
	public class CustomerDetails
	{
		[DataMember]
		public string Id { get; set; }
	}

	[DataContract]
	public class CustomerDetailsResponse : IHasResponseStatus
	{
		public CustomerDetailsResponse()
		{
			this.ResponseStatus = new ResponseStatus();
			this.CustomerOrders = new List<CustomerOrder>();
		}

		[DataMember]
		public Customer Customer { get; set; }

		[DataMember]
		public List<CustomerOrder> CustomerOrders { get; set; }

		[DataMember]
		public ResponseStatus ResponseStatus { get; set; }
	}

}