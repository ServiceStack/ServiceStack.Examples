using NUnit.Framework;
using ServiceStack.Examples.ServiceInterface;
using ServiceStack.Examples.ServiceModel.Operations;
using ServiceStack.Examples.ServiceModel.Types;
using ServiceStack.OrmLite;

namespace ServiceStack.Examples.Tests
{
    [TestFixture]
    public class StoreNewUserTests
        : TestHostBase
    {
        readonly StoreNewUser request = new StoreNewUser
        {
            UserName = "Test",
            Email = "admin@test.com",
            Password = "password"
        };

        [Test]
        public void StoreNewUser_Test()
        {
            using (var db = ConnectionFactory.Open())
            {
                var service = new StoreNewUserService { ConnectionFactory = ConnectionFactory };

                var newUserRequest = new StoreNewUser
                {
                    UserName = "StoreNewUser_Test",
                    Email = "StoreNewUser@test.com",
                    Password = "password"
                };
                var response = (StoreNewUserResponse)service.Execute(newUserRequest);

                var storedUser = db.First<User>("UserName = {0}", newUserRequest.UserName);
                Assert.That(storedUser.Id, Is.EqualTo(response.UserId));
                Assert.That(storedUser.Email, Is.EqualTo(newUserRequest.Email));
                Assert.That(storedUser.Password, Is.EqualTo(newUserRequest.Password));
            }
        }

        [Test]
        public void Existing_user_returns_error_response()
        {
            using (var db = ConnectionFactory.OpenDbConnection())
            {
                db.Insert(new User { UserName = request.UserName });

                var service = new StoreNewUserService { ConnectionFactory = ConnectionFactory };
                var response = (StoreNewUserResponse)service.Execute(request);

                Assert.That(response.ResponseStatus.ErrorCode, Is.EqualTo("UserNameMustBeUnique"));
            }
        }

    }
}