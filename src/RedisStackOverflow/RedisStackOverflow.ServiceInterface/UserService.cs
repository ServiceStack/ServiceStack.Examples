using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace RedisStackOverflow.ServiceInterface
{
	[DataContract]
	[RestService("/users")]
	public class User
	{
		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public string DisplayName { get; set; }
	}

	[DataContract]
	public class UserResponse
	{
		[DataMember]
		public User Result { get; set; }
	}

	public class UserService
	: RestServiceBase<User>
	{
		public IRepository Repository { get; set; }

		public override object OnPost(User request)
		{
			return new UserResponse
			{
				Result = Repository.GetOrCreateUser(request),
			};
		}
	}
}