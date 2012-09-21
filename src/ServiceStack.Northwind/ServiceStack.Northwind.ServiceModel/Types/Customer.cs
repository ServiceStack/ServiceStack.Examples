using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{	
    public class Customer
    {		
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

        public string Email
        {
            get { return this.ContactName.Replace(" ", ".").ToLower() + "@gmail.com"; }
        }
    }
}