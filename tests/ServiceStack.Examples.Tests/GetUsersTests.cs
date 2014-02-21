using NUnit.Framework;
using ServiceStack.Examples.ServiceInterface;
using ServiceStack.Examples.ServiceModel;
using ServiceStack.Examples.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.Testing;

namespace ServiceStack.Examples.Tests
{
	[TestFixture]
	public class GetUsersTests
		: TestHostBase
	{
		[Test]
		public void GetUsers_Test()
		{
            using (var appHost = new BasicAppHost(typeof(GetUsersService).Assembly).Init())
		    {
                var request = new GetUsers
                {
                    UserIds = new ArrayOfLong(1, 2),
                    UserNames = new ArrayOfString("User3", "User4")
                };

                var factory = new OrmLiteConnectionFactory(
                    InMemoryDb, SqliteDialect.Provider);

                using (var db = factory.Open())
                {
                    db.DropAndCreateTable<User>();
                    db.Insert(new User { Id = 1, UserName = "User1" });
                    db.Insert(new User { Id = 2, UserName = "User2" });
                    db.Insert(new User { Id = 3, UserName = "User3" });
                    db.Insert(new User { Id = 4, UserName = "User4" });

                    var handler = appHost.Resolve<GetUsersService>();

                    var response = handler.Any(request);

                    Assert.That(response.Users.Count, Is.EqualTo(4));
                }
            }
		}
	}
}