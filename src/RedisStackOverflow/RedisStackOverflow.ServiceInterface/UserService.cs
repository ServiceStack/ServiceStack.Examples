using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace RedisStackOverflow.ServiceInterface
{
    /// <summary>
    /// Define your ServiceStack web service request (i.e. the Request DTO).
    /// </summary> 
    [Route("/users")]
    public class User
    {
        public long Id { get; set; }		
        public string DisplayName { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    public class UserResponse
    {		
        public User Result { get; set; }
    }

    /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class UserService : RestServiceBase<User>
    {
        /// <summary>
        /// Gets or sets the repository. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
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