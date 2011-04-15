using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace ServiceStack.Examples.ServiceModel.Operations
{
	/// <summary>
	/// Use Plain old DataContract's Define your 'Service Interface'
	/// 
	/// This example introduces the concept of a generic 'ResponseStatus' that 
	/// your service client can use to assert that the request was successful.
	/// The ResponseStatus DTO also enables you to serialize an exception in your service.
	/// </summary>
	public class StoreNewUser
	{
		public string UserName { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }
	}

	public class StoreNewUserResponse
	{
		public StoreNewUserResponse()
		{
			this.ResponseStatus = new ResponseStatus();
		}

		public long UserId { get; set; }

		public ResponseStatus ResponseStatus { get; set; }
	}
}