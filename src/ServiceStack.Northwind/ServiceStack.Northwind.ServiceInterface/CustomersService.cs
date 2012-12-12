using Northwind.ServiceModel.Operations;
using Northwind.ServiceModel.Types;
using ServiceStack.OrmLite;

namespace Northwind.ServiceInterface
{
    /// <summary>
    /// Create your ServiceStack RESTful web service implementation. 
    /// </summary>
    public class CustomersService : ServiceStack.ServiceInterface.Service
    {
        public CustomersResponse Get(Customers request)
        {
            return new CustomersResponse { Customers = base.Db.Select<Customer>() };
        }
    }
}
