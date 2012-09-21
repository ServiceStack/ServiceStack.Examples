using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace ServiceStack.Northwind.ServiceModel.Operations
{    
    [Route("/customers/{Id}")]
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
}