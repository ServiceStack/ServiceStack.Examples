using ServiceStack.Northwind.ServiceModel.Operations;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;

namespace ServiceStack.Northwind.ServiceInterface
{
    /// <summary>
    /// Create your ServiceStack RESTful web service implementation. 
    /// </summary>
    public class CustomersService : RestServiceBase<Customers>
    {
        /// <summary>
        /// Gets or sets the database factory. The built-in IoC used with ServiceStack auto wires this property.
        /// </summary>
        public IDbConnectionFactory DbFactory { get; set; }

        public override object OnGet(Customers request)
        {
            return new CustomersResponse { Customers = DbFactory.Run(dbCmd => dbCmd.Select<Customer>()) };
        }
    }
}
