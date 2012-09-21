using System;
using System.Linq;
using ServiceStack.Common.Extensions;
using ServiceStack.Examples.ServiceModel.Operations;
using ServiceStack.Examples.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace ServiceStack.Examples.ServiceInterface
{
    /// <summary>
    /// The service handler that will be used to execute the request.
    /// 
    /// This example introduces the concept of a generic 'ResponseStatus' that 
    /// your service client can use to assert that the request was successful.
    /// The ResponseStatus DTO also enables you to serialize an exception in your service.
    /// 
    /// Note: This example is kept simple on purpose. In practice you would not persist your DTO's
    /// (i.e. DataContract's) directly. Instead you would use your domain models (aka ORM) for this task. 
    /// </summary>
    public class StoreNewUserService : IService<StoreNewUser>
    {
        //Example of ServiceStack's built-in Funq IOC property injection
        public IDbConnectionFactory ConnectionFactory { get; set; }

        private const string ErrorAlreadyExists = "UserNameMustBeUnique";

        public object Execute(StoreNewUser request)
        {
            using (var dbConn = ConnectionFactory.OpenDbConnection())
            {
                var existingUsers = dbConn.Select<User>("UserName = {0}", request.UserName).ToList();

                if (existingUsers.Count > 0)
                {
                    return new StoreNewUserResponse
                    {
                        ResponseStatus = new ResponseStatus {
                            ErrorCode = ErrorAlreadyExists,
                            Message = ErrorAlreadyExists.ToEnglish()
                        }
                    };
                }

                var newUser = new User
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    Password = request.Password,
                    GlobalId = Guid.NewGuid(),
                };

                dbConn.Insert(newUser);

                return new StoreNewUserResponse { UserId = dbConn.GetLastInsertId() };
            }
        }
    }
}