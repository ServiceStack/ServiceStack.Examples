using System.Linq;
using ServiceStack.Common.Extensions;
using ServiceStack.Northwind.ServiceModel.Operations;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;

namespace ServiceStack.Northwind.ServiceInterface
{
	public class OrdersService : RestServiceBase<Orders>
	{
		private const int PageCount = 20;

		public IDbConnectionFactory DbFactory { get; set; }

		public override object OnGet(Orders request)
		{
			using (var dbConn = DbFactory.OpenDbConnection())
			using (var dbCmd = dbConn.CreateCommand())
			{
				var orders = request.CustomerId.IsNullOrEmpty()
					? dbCmd.Select<Order>("ORDER BY OrderDate DESC LIMIT {0}, {1}", (request.Page.GetValueOrDefault(1) - 1) * PageCount, PageCount)
					: dbCmd.Select<Order>("CustomerId = {0}", request.CustomerId);

				if (orders.Count == 0) 
					return new OrdersResponse();

				var orderDetails = dbCmd.Select<OrderDetail>(
					"OrderId IN ({0})", new SqlInValues(orders.ConvertAll(x => x.Id)));

				var orderDetailsLookup = orderDetails.ToLookup(o => o.OrderId);

				var customerOrders = orders.ConvertAll(o =>
					new CustomerOrder
					{
						Order = o,
						OrderDetails = orderDetailsLookup[o.Id].ToList()
					});

				return new OrdersResponse { Results = customerOrders };
			}
		}
	}
}
