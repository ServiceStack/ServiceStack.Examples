using ServiceStack.Northwind.ServiceModel.Operations;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.OrmLite;

namespace ServiceStack.Northwind.ServiceInterface
{
    public class CustomersService : ServiceStack.Service
    {
        public CustomersResponse Get(Customers request)
        {
            return new CustomersResponse { Customers = Db.Select<Customer>() };
        }
    }
}
