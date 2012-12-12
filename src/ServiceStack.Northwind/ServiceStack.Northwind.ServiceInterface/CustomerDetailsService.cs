using System;
using System.Net;
using ServiceStack.Common.Web;
using Northwind.ServiceModel.Operations;
using Northwind.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;

namespace Northwind.ServiceInterface
{
    /// <summary>
    /// Create your ServiceStack RESTful web service implementation. 
    /// </summary>
    public class CustomerDetailsService : Service
    {
        public CustomerDetailsResponse Get(CustomerDetails request)
        {
            var customer = Db.IdOrDefault<Customer>(request.Id);
            if (customer == null)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Customer does not exist: " + request.Id));

            using (var ordersService = base.ResolveService<OrdersService>())
            {
                var ordersResponse = ordersService.Get(new Orders { CustomerId = customer.Id });

                return new CustomerDetailsResponse {
                    Customer = customer,
                    CustomerOrders = ordersResponse.Results,
                };
            }
        }
    }
}
