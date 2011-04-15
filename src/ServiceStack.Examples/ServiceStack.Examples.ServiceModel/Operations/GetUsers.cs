using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;
using ArrayOfUser=ServiceStack.Examples.ServiceModel.Types.ArrayOfUser;

namespace ServiceStack.Examples.ServiceModel.Operations
{
	/// <summary>
	/// Use Plain old DataContract's Define your 'Service Interface'
	/// 
	/// This example shows the flavour of SOA-style webservices. 
	/// i.e. group similar operations into a single batch-full service request.
	/// </summary>
	public class GetUsers
	{
		public List<long> UserIds { get; set; }

		public List<string> UserNames { get; set; }
	}

	public class GetUsersResponse
	{
		public GetUsersResponse()
		{
			this.ResponseStatus = new ResponseStatus();
		}

		public ArrayOfUser Users { get; set; }

		public ResponseStatus ResponseStatus { get; set; }
	}
}