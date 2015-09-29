using System.Collections.Generic;
using ServiceStack.Examples.ServiceModel;
using ServiceStack.Examples.ServiceModel.Types;
using ServiceStack.OrmLite;

namespace ServiceStack.Examples.ServiceInterface
{
    /// <summary>
    /// The service handler that will be used to execute the request.
    /// 
    /// This example shows a simple introduction into SOA-like webservices. 
    /// i.e. group similar operations into a single 'document-centric like' service request.
    /// </summary>
    public class GetUsersService : Service
    {
        public GetUsersResponse Any(GetUsers request)
        {
            var users = new List<User>();

            if (request.UserIds != null && request.UserIds.Count > 0)
            {
                users.AddRange(Db.SelectByIds<User>(request.UserIds));
            }

            if (request.UserNames != null && request.UserNames.Count > 0)
            {
                users.AddRange(Db.Select<User>(q => request.UserNames.Contains(q.UserName)));
            }

            return new GetUsersResponse { Users = new ArrayOfUser(users) };
        }
    }
}