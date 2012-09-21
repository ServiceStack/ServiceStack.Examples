using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DesignPatterns.Model;

namespace ServiceStack.Examples.ServiceModel.Types
{
    [DataContract(Namespace = ExampleConfig.DefaultNamespace)]
    public class CustomerOrders
    {
        public CustomerOrders()
        {
            this.Orders = new List<Order>();
        }
        
        public Customer Customer { get; set; }		
        public List<Order> Orders { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as CustomerOrders;
            if (other == null) return false;

            var i = 0;
            return this.Customer.Equals(other.Customer) && 
                this.Orders.All(x => x.Equals(other.Orders[i++]));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [DataContract(Namespace = ExampleConfig.DefaultNamespace)]
    public class Customer : IHasStringId
    {
        public Customer() { }

        public Customer(string customerId, string companyName, string contactName, string contactTitle,
                        string address, string city, string region, string postalCode, string country,
                        string phoneNo, string faxNo,
                        byte[] picture)
        {
            Id = customerId;
            CompanyName = companyName;
            ContactName = contactName;
            ContactTitle = contactTitle;
            Address = address;
            City = city;
            Region = region;
            PostalCode = postalCode;
            Country = country;
            Phone = phoneNo;
            Fax = faxNo;
            Picture = picture;
        }
        
        public string Id { get; set; }		
        public string CompanyName { get; set; }		
        public string ContactName { get; set; }		
        public string ContactTitle { get; set; }		
        public string Address { get; set; }		
        public string City { get; set; }		
        public string Region { get; set; }		
        public string PostalCode { get; set; }		
        public string Country { get; set; }		
        public string Phone { get; set; }		
        public string Fax { get; set; }
        public byte[] Picture { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Customer;
            if (other == null) return false;

            return this.Address == other.Address
                   && this.City == other.City
                   && this.CompanyName == other.CompanyName
                   && this.ContactName == other.ContactName
                   && this.ContactTitle == other.ContactTitle
                   && this.Country == other.Country
                   && this.Fax == other.Fax
                   && this.Id == other.Id
                   && this.Phone == other.Phone
                   && this.Picture == other.Picture
                   && this.PostalCode == other.PostalCode
                   && this.Region == other.Region;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [DataContract(Namespace = ExampleConfig.DefaultNamespace)]
    public class Order
    {
        public Order()
        {
            this.OrderDetails = new List<OrderDetail>();
        }
        
        public OrderHeader OrderHeader { get; set; }		
        public List<OrderDetail> OrderDetails { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Order;
            if (other == null) return false;

            var i = 0;
            return this.OrderHeader.Equals(other.OrderHeader)
                   && this.OrderDetails.All(x => x.Equals(other.OrderDetails[i++]));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [DataContract(Namespace = ExampleConfig.DefaultNamespace)]
    public class OrderHeader : IHasIntId
    {
        public OrderHeader() { }

        public OrderHeader(
            int orderId, string customerId, int employeeId, DateTime? orderDate, DateTime? requiredDate,
            DateTime? shippedDate, int shipVia, decimal freight, string shipName,
            string address, string city, string region, string postalCode, string country)
        {
            Id = orderId;
            CustomerId = customerId;
            EmployeeId = employeeId;
            OrderDate = orderDate;
            RequiredDate = requiredDate;
            ShippedDate = shippedDate;
            ShipVia = shipVia;
            Freight = freight;
            ShipName = shipName;
            ShipAddress = address;
            ShipCity = city;
            ShipRegion = region;
            ShipPostalCode = postalCode;
            ShipCountry = country;
        }
        
        public int Id { get; set; }		
        public string CustomerId { get; set; }		
        public int EmployeeId { get; set; }		
        public DateTime? OrderDate { get; set; }		
        public DateTime? RequiredDate { get; set; }		
        public DateTime? ShippedDate { get; set; }		
        public int? ShipVia { get; set; }		
        public decimal Freight { get; set; }		
        public string ShipName { get; set; }		
        public string ShipAddress { get; set; }		
        public string ShipCity { get; set; }		
        public string ShipRegion { get; set; }		
        public string ShipPostalCode { get; set; }		
        public string ShipCountry { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as OrderHeader;
            if (other == null) return false;

            return this.Id == other.Id
                   && this.CustomerId == other.CustomerId
                   && this.EmployeeId == other.EmployeeId
                   && this.OrderDate == other.OrderDate
                   && this.RequiredDate == other.RequiredDate
                   && this.ShippedDate == other.ShippedDate
                   && this.ShipVia == other.ShipVia
                   && this.Freight == other.Freight
                   && this.ShipName == other.ShipName
                   && this.ShipAddress == other.ShipAddress
                   && this.ShipCity == other.ShipCity
                   && this.ShipRegion == other.ShipRegion
                   && this.ShipPostalCode == other.ShipPostalCode
                   && this.ShipCountry == other.ShipCountry;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [DataContract(Namespace = ExampleConfig.DefaultNamespace)]
    public class OrderDetail : IHasStringId
    {
        public OrderDetail() { }

        public OrderDetail(
            int orderId, int productId, decimal unitPrice, short quantity, double discount)
        {
            OrderId = orderId;
            ProductId = productId;
            UnitPrice = unitPrice;
            Quantity = quantity;
            Discount = discount;
        }

        public string Id { get { return this.OrderId + "/" + this.ProductId; } }		
        public int OrderId { get; set; }		
        public int ProductId { get; set; }		
        public decimal UnitPrice { get; set; }		
        public short Quantity { get; set; }		
        public double Discount { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as OrderDetail;
            if (other == null) return false;

            return this.Id == other.Id
                   && this.OrderId == other.OrderId
                   && this.ProductId == other.ProductId
                   && this.UnitPrice == other.UnitPrice
                   && this.Quantity == other.Quantity
                   && this.Discount == other.Discount;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}