using ServiceStack.Northwind.ServiceModel.Operations;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;

namespace ServiceStack.Northwind.ServiceInterface
{
	public class CustomersService : RestServiceBase<Customers>
	{
		public IDbConnectionFactory DbFactory { get; set; }

		public override object OnGet(Customers request)
		{
			return new CustomersResponse { Customers = DbFactory.Exec(dbCmd => dbCmd.Select<Customer>()) };
		}
	}

	public class CustomerService : RestServiceBase<Customer>
	{
		public IDbConnectionFactory DbFactory { get; set; }

		public override object OnPost(Customer request)
		{
			return new CustomersResponse { Customers = DbFactory.Exec(dbCmd => dbCmd.Select<Customer>()) };
		}
	}
}
