using System;
using ServiceStack.Examples.ServiceModel;
using ServiceStack.Examples.ServiceModel.Types;
using ServiceStack.OrmLite;

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
    public class StoreNewUserService : Service
    {
        private const string ErrorAlreadyExists = "UserNameMustBeUnique";

        public StoreNewUserResponse Any(StoreNewUser request)
        {
            var existingUsers = Db.Select<User>(q => q.UserName == request.UserName);

            if (existingUsers.Count > 0)
            {
                return new StoreNewUserResponse
                {
                    ResponseStatus = new ResponseStatus
                    {
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

            Db.Save(newUser);

            return new StoreNewUserResponse { UserId = newUser.Id };
        }
    }
}