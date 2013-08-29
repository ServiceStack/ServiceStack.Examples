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

}using System.Runtime.Serialization;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.ServiceInterface.ServiceModel;

namespace ServiceStack.Northwind.ServiceModel.Operations
{
	[DataContract]
	public class CustomerResponse : IHasResponseStatus
	{
		public CustomerResponse()
		{
			this.ResponseStatus = new ResponseStatus();
		}

		[DataMember]
		public Customer Customer { get; set; }

		[DataMember]
		public ResponseStatus ResponseStatus { get; set; }
	}

}using System.Collections.Generic;
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

}using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace ServiceStack.Northwind.ServiceModel.Operations
{
	[DataContract]
	[RestService("/customers")]
	public class Customers {}

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

}public class Customers {}
public class CustomerResponse : IHasResponseStatus
{
	public CustomerResponse()
	{
		this.ResponseStatus = new ResponseStatus();
	}
	
	public Customer Customer { get; set; }
	
	public ResponseStatus ResponseStatus { get; set; }
}
public class CustomersResponse : IHasResponseStatus
{
	public CustomersResponse()
	{
		this.ResponseStatus = new ResponseStatus();
		this.Customers = new List<Customer>();
	}
	
	public List<Customer> Customers { get; set; }
	
	public ResponseStatus ResponseStatus { get; set; }
}
public class CustomerDetails
{
	
	public string Id { get; set; }
}


public class CustomerDetailsResponse : IHasResponseStatus
{
	public CustomerDetailsResponse()
	{
		this.ResponseStatus = new ResponseStatus();
		this.CustomerOrders = new List<CustomerOrder>();
	}
	
	public Customer Customer { get; set; }
	
	public List<CustomerOrder> CustomerOrders { get; set; }
	
	public ResponseStatus ResponseStatus { get; set; }
}
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


