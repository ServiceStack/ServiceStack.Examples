using System.Net;
using RestIntro.ServiceModel;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;

namespace RestIntro.ServiceInterface
{
	public class CustomerService : RestServiceBase<Customer>
	{
		public IDbConnectionFactory DbFactory { get; set; }

		public override object OnGet(Customer request)
		{
			if (request.Id != default(long))
				return DbFactory.Exec(dbCmd => dbCmd.GetById<Customer>(request.Id));

			return DbFactory.Exec(dbCmd => dbCmd.Select<Customer>());
		}

		public override object OnPost(Customer customer)
		{
			DbFactory.Exec(dbCmd =>
			{
				dbCmd.Save(customer);
				customer.Id = (int)dbCmd.GetLastInsertId();
			});

			var pathToNewResource = base.RequestContext.AbsoluteUri.WithTrailingSlash() + customer.Id;
			return new HttpResult(customer) {
				StatusCode = HttpStatusCode.Created,
				Headers = {
					{ HttpHeaders.Location, pathToNewResource },
				}
			};
		}

		public override object OnPut(Customer customer)
		{
			DbFactory.Exec(dbCmd => dbCmd.Save(customer));
			return customer;
		}

		public override object OnDelete(Customer request)
		{
			DbFactory.Exec(dbCmd => dbCmd.DeleteById<Customer>(request.Id));
			return null;
		}
	}
}