using ServiceStack.Examples.ServiceModel;
using ServiceStack.Examples.ServiceModel.Types;
using ServiceStack.OrmLite;

namespace ServiceStack.Examples.ServiceInterface
{
    public class GetAllUsersService : Service
    {
        public object Any(GetAllUsers request)
        {
            var users = Db.Select<User>();
            return new GetAllUsersResponse { Users = new ArrayOfUser(users) };
        }
    }

}