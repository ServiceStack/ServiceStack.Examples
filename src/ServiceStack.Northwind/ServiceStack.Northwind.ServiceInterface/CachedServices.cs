


namespace ServiceStack.Northwind.ServiceInterface
{
    using Caching;
    using ServiceModel.Operations;

    public class CachedCustomersService : Service
	{
		public ICacheClient CacheClient { get; set; }

		public object Get(CachedCustomers request)
		{
			return base.Request.ToOptimizedResultUsingCache(
				this.CacheClient, "urn:customers", () => {
					var service = this.ResolveService<CustomersService>();
					return service.Get(new Customers());
				});
		}
	}

	public class CachedCustomerDetailsService : Service
	{
		public ICacheClient CacheClient { get; set; }

		public object Get(CachedCustomerDetails request)
		{
			var cacheKey = UrnId.Create<CustomerDetails>(request.Id);
			return base.Request.ToOptimizedResultUsingCache(
				this.CacheClient, cacheKey, () => 
                    this.ResolveService<CustomerDetailsService>().Get(new CustomerDetails { Id = request.Id }));
		}
	}

	public class CachedOrdersService : Service
	{
		public ICacheClient CacheClient { get; set; }

		public object Get(CachedOrders request)
		{
			var cacheKey = UrnId.Create<Orders>(request.CustomerId ?? "all", request.Page.GetValueOrDefault(0).ToString());
			return base.Request.ToOptimizedResultUsingCache(CacheClient, cacheKey, 
				() => (OrdersResponse) ResolveService<OrdersService>()
					.Get(new Orders { CustomerId = request.CustomerId, Page = request.Page }));
		}
	}

}
