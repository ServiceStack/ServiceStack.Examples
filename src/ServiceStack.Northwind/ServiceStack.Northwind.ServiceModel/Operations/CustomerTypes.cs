using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{
	[DataContract]
	public class Category
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string CategoryName { get; set; }
		[DataMember]
		public string Description { get; set; }
	}
}using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{
	[DataContract]
	public class Customer
	{
		[DataMember]
		public string Id { get; set; }

		[DataMember]
		public string CompanyName { get; set; }
		
		[DataMember]
		public string ContactName { get; set; }
		
		[DataMember]
		public string ContactTitle { get; set; }
		
		[DataMember]
		public string Address { get; set; }
		
		[DataMember]
		public string City { get; set; }
		
		[DataMember]
		public string Region { get; set; }
		
		[DataMember]
		public string PostalCode { get; set; }
		
		[DataMember]
		public string Country { get; set; }
		
		[DataMember]
		public string Phone { get; set; }
		
		[DataMember]
		public string Fax { get; set; }

		public string Email
		{
			get { return this.ContactName.Replace(" ", ".").ToLower() + "@gmail.com"; }
		}
	}
}using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{
	[DataContract]
	public class CustomerCustomerDemo
	{
		[DataMember]
		public string Id { get; set; }
		[DataMember]
		public string CustomerTypeId { get; set; }
	}
}using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{
	[DataContract]
	public class CustomerDemographic
	{
		[DataMember]
		public string Id { get; set; }
		[DataMember]
		public string CustomerDesc { get; set; }
	}
}using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{
	[DataContract]
	public class CustomerOrder
	{
		public CustomerOrder()
		{
			this.OrderDetails = new List<OrderDetail>();
		}

		[DataMember]
		public Order Order { get; set; }

		[DataMember]
		public List<OrderDetail> OrderDetails { get; set; }
	}
}using System;
using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{
	[DataContract]
	public class Employee
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string LastName { get; set; }
		[DataMember]
		public string FirstName { get; set; }
		[DataMember]
		public string Title { get; set; }
		[DataMember]
		public string TitleOfCourtesy { get; set; }
		[DataMember]
		public DateTime? BirthDate { get; set; }
		[DataMember]
		public DateTime? HireDate { get; set; }
		[DataMember]
		public string Address { get; set; }
		[DataMember]
		public string City { get; set; }
		[DataMember]
		public string Region { get; set; }
		[DataMember]
		public string PostalCode { get; set; }
		[DataMember]
		public string Country { get; set; }
		[DataMember]
		public string HomePhone { get; set; }
		[DataMember]
		public string Extension { get; set; }

		public byte[] Photo { get; set; }
		[DataMember]
		public string Notes { get; set; }
		[DataMember]
		public int? ReportsTo { get; set; }
		[DataMember]
		public string PhotoPath { get; set; }
	}
}using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{
	[DataContract]
	public class EmployeeTerritory
	{
		public string Id { get { return this.EmployeeId + "/" + this.TerritoryId; } }
		[DataMember]
		public int EmployeeId { get; set; }
		[DataMember]
		public string TerritoryId { get; set; }
	}
}using System;
using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{
	[DataContract]
	public class Order
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string CustomerId { get; set; }
		[DataMember]
		public int EmployeeId { get; set; }
		[DataMember]
		public DateTime? OrderDate { get; set; }
		[DataMember]
		public DateTime? RequiredDate { get; set; }
		[DataMember]
		public DateTime? ShippedDate { get; set; }
		[DataMember]
		public int? ShipVia { get; set; }
		[DataMember]
		public decimal Freight { get; set; }
		[DataMember]
		public string ShipName { get; set; }
		[DataMember]
		public string ShipAddress { get; set; }
		[DataMember]
		public string ShipCity { get; set; }
		[DataMember]
		public string ShipRegion { get; set; }
		[DataMember]
		public string ShipPostalCode { get; set; }
		[DataMember]
		public string ShipCountry { get; set; }
	}
}using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{
	[DataContract]
	public class OrderDetail
	{
		public string Id { get { return this.OrderId + "/" + this.ProductId; } }

		[DataMember]
		public int OrderId { get; set; }

		[DataMember]
		public int ProductId { get; set; }

		[DataMember]
		public decimal UnitPrice { get; set; }

		[DataMember]
		public short Quantity { get; set; }

		[DataMember]
		public double Discount { get; set; }
	}
}using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{
	[DataContract]
	public class Product
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string ProductName { get; set; }
		[DataMember]
		public int SupplierId { get; set; }
		[DataMember]
		public int CategoryId { get; set; }
		[DataMember]
		public string QuantityPerUnit { get; set; }
		[DataMember]
		public decimal UnitPrice { get; set; }
		[DataMember]
		public short UnitsInStock { get; set; }
		[DataMember]
		public short UnitsOnOrder { get; set; }
		[DataMember]
		public short ReorderLevel { get; set; }
		[DataMember]
		public bool Discontinued { get; set; }
	}
}using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{
	[DataContract]
	public class Region
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string RegionDescription { get; set; }
	}
}using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{
	[DataContract]
	public class Shipper
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string CompanyName { get; set; }
		[DataMember]
		public string Phone { get; set; }
	}
}using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{
	[DataContract]
	public class Supplier
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string CompanyName { get; set; }
		[DataMember]
		public string ContactName { get; set; }
		[DataMember]
		public string ContactTitle { get; set; }
		[DataMember]
		public string Address { get; set; }
		[DataMember]
		public string City { get; set; }
		[DataMember]
		public string Region { get; set; }
		[DataMember]
		public string PostalCode { get; set; }
		[DataMember]
		public string Country { get; set; }
		[DataMember]
		public string Phone { get; set; }
		[DataMember]
		public string Fax { get; set; }
		[DataMember]
		public string HomePage { get; set; }
	}
}using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{
	[DataContract]
	public class Territory
	{
		[DataMember]
		public string Id { get; set; }
		[DataMember]
		public string TerritoryDescription { get; set; }
		[DataMember]
		public int RegionId { get; set; }
	}
}