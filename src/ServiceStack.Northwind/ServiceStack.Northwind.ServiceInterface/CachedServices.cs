using ServiceStack.CacheAccess;
using ServiceStack.Common;
using ServiceStack.Northwind.ServiceModel.Operations;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace ServiceStack.Northwind.ServiceInterface
{
    /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class CachedCustomersService : RestServiceBase<CachedCustomers>
    {
        /// <summary>
        /// Gets or sets the cache client. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
        public ICacheClient CacheClient { get; set; }

        public override object OnGet(CachedCustomers request)
        {
            //Manually create the Unified Resource Name "urn:customers".
            return base.RequestContext.ToOptimizedResultUsingCache(
                this.CacheClient, "urn:customers", () =>
                {
                    //Resolve the service in order to get the customers.
                    var service = this.ResolveService<CustomersService>();
                    return (CustomersResponse)service.Get(new Customers());
                });
        }
    }

    /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class CachedCustomerDetailsService : RestServiceBase<CachedCustomerDetails>
    {
        /// <summary>
        /// Gets or sets the cache client. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
        public ICacheClient CacheClient { get; set; }

        public override object OnGet(CachedCustomerDetails request)
        {
            //Create the Unified Resource Name "urn:customerdetails:{id}".
            var cacheKey = UrnId.Create<CustomerDetails>(request.Id);
            return base.RequestContext.ToOptimizedResultUsingCache(
                this.CacheClient, cacheKey, () =>
                {
                    return (CustomerDetailsResponse)this.ResolveService<CustomerDetailsService>()
                        .Get(new CustomerDetails { Id = request.Id });
                });
        }
    }

    /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class CachedOrdersService : RestServiceBase<CachedOrders>
    {
        /// <summary>
        /// Gets or sets the cache client. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
        public ICacheClient CacheClient { get; set; }

        public override object OnGet(CachedOrders request)
        {
            var cacheKey = UrnId.Create<Orders>(request.CustomerId ?? "all", request.Page.GetValueOrDefault(0).ToString());
            return base.RequestContext.ToOptimizedResultUsingCache(this.CacheClient, cacheKey, () =>
                {
                    return (OrdersResponse)this.ResolveService<OrdersService>()
                        .Get(new Orders { CustomerId = request.CustomerId, Page = request.Page });
                });
        }
    }

}
