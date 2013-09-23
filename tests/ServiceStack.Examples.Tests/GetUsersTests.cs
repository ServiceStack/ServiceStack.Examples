using NUnit.Framework;
using ServiceStack.Examples.ServiceInterface;
using ServiceStack.Examples.ServiceModel.Operations;
using ServiceStack.Examples.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;
using ServiceStack.ServiceInterface.ServiceModel;

namespace ServiceStack.Examples.Tests
{
	[TestFixture]
	public class GetUsersTests
		: TestHostBase
	{
		[Test]
		public void GetUsers_Test()
		{
			var request = new GetUsers
			{
				UserIds = new ArrayOfLong(1, 2),
				UserNames = new ArrayOfString("User3", "User4")
			};

			var factory = new OrmLiteConnectionFactory(
				InMemoryDb, false, SqliteDialect.Provider);

            using (var db = factory.Open())
			{
				db.DropAndCreateTable<User>();
				db.Insert(new User { Id = 1, UserName = "User1" });
				db.Insert(new User { Id = 2, UserName = "User2" });
				db.Insert(new User { Id = 3, UserName = "User3" });
				db.Insert(new User { Id = 4, UserName = "User4" });

				var handler = new GetUsersService { ConnectionFactory = factory };

				var response = (GetUsersResponse)handler.Execute(request);

				Assert.That(response.Users.Count, Is.EqualTo(4));
			}
		}
	}
}