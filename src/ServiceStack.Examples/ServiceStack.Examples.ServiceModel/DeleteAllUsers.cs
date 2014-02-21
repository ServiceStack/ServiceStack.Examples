using System.Runtime.Serialization;

namespace ServiceStack.Examples.ServiceModel
{
	/// <summary>
	/// Use Plain old DataContract's Define your 'Service Interface'
	/// </summary>
    [DataContract(Namespace = ExampleConfig.DefaultNamespace)]
    public class DeleteAllUsers
	{
	}

    [DataContract(Namespace = ExampleConfig.DefaultNamespace)]
    public class DeleteAllUsersResponse
	{
		public DeleteAllUsersResponse()
		{
			this.ResponseStatus = new ResponseStatus();
		}

        [DataMember]
        public long UserId { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }
	}
}