using RestIntro.ServiceModel;
using ServiceStack;
using ServiceStack.OrmLite;

namespace RestIntro.ServiceInterface
{
    /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class CustomerService : Service
    {
        public object Get(Customer request)
        {
            if (request.Id != default(long))
                return Db.SingleById<Customer>(request.Id);

            return Db.Select<Customer>();
        }

        public object Post(Customer customer)
        {
            Db.Save(customer);

            var pathToNewResource = base.Request.AbsoluteUri.CombineWith(customer.Id.ToString());
            return HttpResult.Status201Created(customer, pathToNewResource);
        }

        public Customer Put(Customer customer)
        {
            Db.Save(customer);
            return customer;
        }

        public void Delete(Customer request)
        {
            Db.DeleteById<Customer>(request.Id);
        }
    }
}