using ServiceStack.Common;
using Northwind.ServiceModel.Operations;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace Northwind.ServiceInterface
{
    /// <summary>
    /// Create your ServiceStack RESTful web service implementation. 
    /// </summary>
    public class CachedCustomersService : Service
    {
        public object Get(CachedCustomers request)
        {
            //Manually create the Unified Resource Name "urn:customers".
            return base.RequestContext.ToOptimizedResultUsingCache(base.Cache, "urn:customers", () =>
            {
                //Resolve the service in order to get the customers.
                using (var service = this.ResolveService<CustomersService>())
                    return service.Get(new Customers());
            });
        }

        public object Get(CachedCustomerDetails request)
        {
            //Create the Unified Resource Name "urn:customerdetails:{id}".
            var cacheKey = UrnId.Create<CustomerDetails>(request.Id);
            return base.RequestContext.ToOptimizedResultUsingCache(base.Cache, cacheKey, () =>
            {
                using (var service = this.ResolveService<CustomerDetailsService>())
                {
                    return service.Get(new CustomerDetails { Id = request.Id });
                }
            });
        }

        public object Get(CachedOrders request)
        {
            var cacheKey = UrnId.Create<Orders>(request.CustomerId ?? "all", request.Page.GetValueOrDefault(0).ToString());
            return base.RequestContext.ToOptimizedResultUsingCache(this.Cache, cacheKey, () => 
            {
                using(var service = this.ResolveService<OrdersService>())
                {
                    return service.Get(new Orders { CustomerId = request.CustomerId, Page = request.Page });
                }
            });
        }
    }

}
