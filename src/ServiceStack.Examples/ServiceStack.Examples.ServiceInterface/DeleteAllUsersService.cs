using ServiceStack.Examples.ServiceModel;
using ServiceStack.Examples.ServiceModel.Types;
using ServiceStack.OrmLite;

namespace ServiceStack.Examples.ServiceInterface
{
    /// <summary>
    /// The service or 'Port' handler that will be used to execute the request.
    /// 
    /// The 'Port' attribute is used to link the 'service request' to the 'service implementation'
    /// </summary>
    public class DeleteAllUsersService : Service
    {
        public object Any(DeleteAllUsers request)
        {
            Db.DeleteAll<User>();

            return new DeleteAllUsersResponse();
        }
    }
}