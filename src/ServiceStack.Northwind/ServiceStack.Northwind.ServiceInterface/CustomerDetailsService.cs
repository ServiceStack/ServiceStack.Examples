using System;
using System.Net;
using ServiceStack.Common.Web;
using ServiceStack.Northwind.ServiceModel.Operations;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;

namespace ServiceStack.Northwind.ServiceInterface
{
    /// <summary>
    /// Create your ServiceStack RESTful web service implementation. 
    /// </summary>
    public class CustomerDetailsService : RestServiceBase<CustomerDetails>
    {
        /// <summary>
        /// Gets or sets the database factory. The built-in IoC used with ServiceStack auto wires this property.
        /// </summary>
        public IDbConnectionFactory DbFactory { get; set; }

        public override object OnGet(CustomerDetails request)
        {
            var customer = DbFactory.Run(dbCmd => dbCmd.GetByIdOrDefault<Customer>(request.Id));
            if (customer == null)
                throw new HttpError(HttpStatusCode.NotFound,
                    new ArgumentException("Customer does not exist: " + request.Id));

            var ordersService = base.ResolveService<OrdersService>();
            var ordersResponse = (OrdersResponse)ordersService.Get(new Orders { CustomerId = customer.Id });

            return new CustomerDetailsResponse
            {
                Customer = customer,
                CustomerOrders = ordersResponse.Results,
            };
        }
    }
}
