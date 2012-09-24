using System.Linq;
using ServiceStack.Common.Extensions;
using ServiceStack.Northwind.ServiceModel.Operations;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;

namespace ServiceStack.Northwind.ServiceInterface
{
    /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class OrdersService : RestServiceBase<Orders>
    {
        private const int PageCount = 20;

        /// <summary>
        /// Gets or sets the database factory. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
        public IDbConnectionFactory DbFactory { get; set; }

        public override object OnGet(Orders request)
        {
            using (var dbConn = DbFactory.OpenDbConnection())
            {
                var orders = request.CustomerId.IsNullOrEmpty()
                    ? dbConn.Select<Order>("ORDER BY OrderDate DESC LIMIT {0}, {1}", (request.Page.GetValueOrDefault(1) - 1) * PageCount, PageCount)
                    : dbConn.Select<Order>("CustomerId = {0}", request.CustomerId);

                if (orders.Count == 0) { return new OrdersResponse(); }

                var orderDetails = dbConn.Select<OrderDetail>(
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
