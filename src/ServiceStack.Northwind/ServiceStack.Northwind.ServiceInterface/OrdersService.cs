using System.Linq;
using ServiceStack.Common.Extensions;
using Northwind.ServiceModel.Operations;
using Northwind.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;

namespace Northwind.ServiceInterface
{
    /// <summary>
    /// Create your ServiceStack RESTful web service implementation. 
    /// </summary>
    public class OrdersService : Service
    {
        private const int PageCount = 20;

        public OrdersResponse Get(Orders request)
        {
            var orders = request.CustomerId.IsNullOrEmpty()
                ? Db.Select<Order>("ORDER BY OrderDate DESC LIMIT {0}, {1}", (request.Page.GetValueOrDefault(1) - 1) * PageCount, PageCount)
                : Db.Select<Order>("CustomerId = {0}", request.CustomerId);

            if (orders.Count == 0) { return new OrdersResponse(); }

            var orderDetails = Db.Select<OrderDetail>(
                "OrderId IN ({0})", new SqlInValues(orders.ConvertAll(x => x.Id)));

            var orderDetailsLookup = orderDetails.ToLookup(o => o.OrderId);

            var customerOrders = orders.ConvertAll(o =>
                new CustomerOrder {
                    Order = o,
                    OrderDetails = orderDetailsLookup[o.Id].ToList()
                });

            return new OrdersResponse { Results = customerOrders };
        }
    }
}
