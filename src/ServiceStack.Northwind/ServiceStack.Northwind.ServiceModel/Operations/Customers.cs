using System.Collections.Generic;
using Northwind.ServiceModel.Types;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Northwind.ServiceModel.Operations
{    
    [Route("/customers")]
    public class Customers { }
    
    public class CustomersResponse
    {
        public CustomersResponse()
        {
            this.ResponseStatus = new ResponseStatus();
            this.Customers = new List<Customer>();
        }
        
        public List<Customer> Customers { get; set; }        
        public ResponseStatus ResponseStatus { get; set; }
    }
}