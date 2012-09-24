using ServiceStack.Examples.ServiceModel.Operations;
using ServiceStack.Examples.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;

namespace ServiceStack.Examples.ServiceInterface
{
    public class GetAllUsersService : IService<GetAllUsers>
    {
        //Example of ServiceStack's IOC property injection
        public IDbConnectionFactory ConnectionFactory { get; set; }

        public object Execute(GetAllUsers request)
        {
            using (var dbConn = ConnectionFactory.OpenDbConnection())
            {
                var users = dbConn.Select<User>();
                return new GetAllUsersResponse { Users = new ArrayOfUser(users) };
            }
        }
    }

}