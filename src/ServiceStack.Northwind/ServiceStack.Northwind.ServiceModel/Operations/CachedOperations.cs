using ServiceStack.ServiceHost;

namespace ServiceStack.Northwind.ServiceModel.Operations
{    
    [Route("/cached/customers")]
    public class CachedCustomers {}
    
    [Route("/cached/customers/{Id}")]
    public class CachedCustomerDetails
    {        
        public string Id { get; set; }
    }
    
    [Route("/cached/orders")]
    [Route("/cached/orders/page/{Page}")]
    [Route("/cached/customers/{CustomerId}/orders")]
    public class CachedOrders
    {        
        public int? Page { get; set; }        
        public string CustomerId { get; set; }
    }
}