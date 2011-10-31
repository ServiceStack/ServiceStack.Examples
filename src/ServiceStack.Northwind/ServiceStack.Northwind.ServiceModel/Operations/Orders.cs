using System.Collections.Generic;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.ServiceInterface.ServiceModel;

public class Customers {}
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


