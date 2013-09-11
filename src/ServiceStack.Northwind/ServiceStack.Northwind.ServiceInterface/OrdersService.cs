using System.Collections.Generic;
using System.Linq;
using ServiceStack.Common.Extensions;
using ServiceStack.Northwind.ServiceModel.Operations;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;

namespace ServiceStack.Northwind.ServiceInterface
{
    public class OrdersService : Service
    {
        private const int PageCount = 20;

        public object Get(Orders request)
        {
            var orders = request.CustomerId.IsNullOrEmpty()
                ? Db.Select<Order>(order => order.OrderByDescending(o => o.OrderDate))
                      .Skip((request.Page.GetValueOrDefault(1) - 1)*PageCount)
                      .Take(PageCount)
                      .ToList()
                : Db.Select<Order>(order => order.Where(o => o.CustomerId == request.CustomerId));

            if (orders.Count == 0)
                return new OrdersResponse();

            var orderDetails = Db.Select<OrderDetail>(detail => Sql.In(detail.OrderId, orders.ConvertAll(x => x.Id)));

            var orderDetailsLookup = orderDetails.ToLookup(o => o.OrderId);

            var customerOrders = orders.ConvertAll(o => new CustomerOrder {
                Order = o,
                OrderDetails = orderDetailsLookup[o.Id].ToList()
            });

            return new OrdersResponse { Results = customerOrders };
        }
    }
}
