using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace ServiceStack.Examples.ServiceModel.Operations
{
	/// <summary>
	/// Use Plain old DataContract's Define your 'Service Interface'
	/// </summary>
	public class DeleteAllUsers
	{
	}

	public class DeleteAllUsersResponse
	{
		public DeleteAllUsersResponse()
		{
			this.ResponseStatus = new ResponseStatus();
		}

		public long UserId { get; set; }

		public ResponseStatus ResponseStatus { get; set; }
	}
}