namespace ServiceStack.Northwind.ServiceInterface
{
	using System.Collections.Generic;
	using System.Linq;
	using ServiceStack.Common.Extensions;
	using ServiceStack.Northwind.ServiceModel.Operations;
	using ServiceStack.Northwind.ServiceModel.Types;
	using ServiceStack.OrmLite;
	using ServiceStack.ServiceInterface;

	public class OrdersService : Service
	{
		private const int PageCount = 20;

		public IDbConnectionFactory DbFactory { get; set; }

		public object Get(Orders request)
		{
			using (var dbConn = DbFactory.OpenDbConnection())
			using (var dbCmd = dbConn.CreateCommand())
			{
				List<Order> orders;

				if (request.CustomerId.IsNullOrEmpty())
				{
					orders = dbCmd.Select<Order>(order => order.OrderByDescending(o => o.OrderDate))
					              .Skip((request.Page.GetValueOrDefault(1) - 1)*PageCount)
					              .Take(PageCount)
					              .ToList();
				}
				else orders = dbCmd.Select<Order>(order => order.Where(o => o.CustomerId == request.CustomerId));

				if (orders.Count == 0)
					return new OrdersResponse();

				var orderDetails = dbCmd.Select<OrderDetail>(detail => Sql.In(detail.OrderId, orders.ConvertAll(x => x.Id)));

				var orderDetailsLookup = orderDetails.ToLookup(o => o.OrderId);

				var customerOrders = orders.ConvertAll(o =>
				                                       new CustomerOrder
					                                       {
						                                       Order = o,
						                                       OrderDetails = orderDetailsLookup[o.Id].ToList()
					                                       });

				return new OrdersResponse {Results = customerOrders};
			}
		}
	}
}