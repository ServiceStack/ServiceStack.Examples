using ServiceStack.Examples.ServiceModel.Operations;
using ServiceStack.Examples.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;

namespace ServiceStack.Examples.ServiceInterface
{
    /// <summary>
    /// The service or 'Port' handler that will be used to execute the request.
    /// 
    /// The 'Port' attribute is used to link the 'service request' to the 'service implementation'
    /// </summary>
    public class DeleteAllUsersService 
        : IService<DeleteAllUsers>
    {
        //Example of ServiceStack's IOC property injection
        public IDbConnectionFactory ConnectionFactory { get; set; }

        public object Execute(DeleteAllUsers request)
        {
            using (var dbConn = ConnectionFactory.OpenDbConnection())
            {
                dbConn.DeleteAll<User>();

                return new DeleteAllUsersResponse();
            }
        }
    }

}