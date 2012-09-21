using System.Collections.Generic;
using ServiceStack.Examples.ServiceModel.Operations;
using ServiceStack.Examples.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;

namespace ServiceStack.Examples.ServiceInterface
{
    /// <summary>
    /// The service handler that will be used to execute the request.
    /// 
    /// This example shows a simple introduction into SOA-like webservices. 
    /// i.e. group similar operations into a single 'document-centric like' service request.
    /// </summary>
    public class GetUsersService : IService<GetUsers>
    {
        //Example of ServiceStack's built-in Funq IOC property injection
        public IDbConnectionFactory ConnectionFactory { get; set; }

        public object Execute(GetUsers request)
        {
            using (var dbConn = ConnectionFactory.OpenDbConnection())
            {
                var users = new List<User>();

                if (request.UserIds != null && request.UserIds.Count > 0)
                {
                    users.AddRange(dbConn.GetByIds<User>(request.UserIds));
                }

                if (request.UserNames != null && request.UserNames.Count > 0)
                {
                    users.AddRange(dbConn.Select<User>(
                        "UserName IN ({0})", request.UserNames.SqlInValues()));
                }

                return new GetUsersResponse { Users = new ArrayOfUser(users) };
            }
        }
    }
}