

namespace ServiceStack.Northwind.ServiceInterface
{
	using ServiceStack.CacheAccess;
	using ServiceStack.Common;
	using ServiceStack.Northwind.ServiceModel.Operations;
	using ServiceStack.ServiceHost;
	using ServiceStack.ServiceInterface;

	public class CachedCustomersService : ServiceStack.ServiceInterface.Service
	{
		public ICacheClient CacheClient { get; set; }

		public object Get(CachedCustomers request)
		{
			return base.RequestContext.ToOptimizedResultUsingCache(
				this.CacheClient, "urn:customers", () => {
					var service = this.ResolveService<CustomersService>();
					return (CustomersResponse) service.Get(new Customers());
				});
		}
	}

	public class CachedCustomerDetailsService : Service
	{
		public ICacheClient CacheClient { get; set; }

		public object Get(CachedCustomerDetails request)
		{
			var cacheKey = UrnId.Create<CustomerDetails>(request.Id);
			return base.RequestContext.ToOptimizedResultUsingCache(
				this.CacheClient, cacheKey, () => (CustomerDetailsResponse)this.ResolveService<CustomerDetailsService>()
				                                                               .Get(new CustomerDetails { Id = request.Id }));
		}
	}

	public class CachedOrdersService : Service
	{
		public ICacheClient CacheClient { get; set; }

		public object Get(CachedOrders request)
		{
			var cacheKey = UrnId.Create<Orders>(request.CustomerId ?? "all", request.Page.GetValueOrDefault(0).ToString());
			return base.RequestContext.ToOptimizedResultUsingCache(CacheClient, cacheKey, 
				() => (OrdersResponse) ResolveService<OrdersService>()
					.Get(new Orders { CustomerId = request.CustomerId, Page = request.Page }));
		}
	}

}
