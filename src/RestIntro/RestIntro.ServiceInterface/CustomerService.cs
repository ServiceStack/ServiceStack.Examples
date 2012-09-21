using System.Net;
using RestIntro.ServiceModel;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;

namespace RestIntro.ServiceInterface
{
    /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class CustomerService : RestServiceBase<Customer>
    {
        /// <summary>
        /// Gets or sets the database factory. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
        public IDbConnectionFactory DbFactory { get; set; }

        public override object OnGet(Customer request)
        {
            if (request.Id != default(long))
                return DbFactory.Run(dbCmd => dbCmd.GetById<Customer>(request.Id));

            return DbFactory.Run(dbCmd => dbCmd.Select<Customer>());
        }

        public override object OnPost(Customer customer)
        {
            DbFactory.Run(dbCmd =>
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
            DbFactory.Run(dbCmd => dbCmd.Save(customer));
            return customer;
        }

        public override object OnDelete(Customer request)
        {
            DbFactory.Run(dbCmd => dbCmd.DeleteById<Customer>(request.Id));
            return null;
        }
    }
}