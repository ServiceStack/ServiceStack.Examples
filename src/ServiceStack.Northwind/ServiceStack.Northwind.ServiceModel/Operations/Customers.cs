using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace ServiceStack.Northwind.ServiceModel.Operations
{    
    [Route("/customers")]
    public class Customers { }
    
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
}