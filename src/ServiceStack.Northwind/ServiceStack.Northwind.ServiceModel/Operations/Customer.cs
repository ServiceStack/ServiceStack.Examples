using System.Runtime.Serialization;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.ServiceInterface.ServiceModel;

namespace ServiceStack.Northwind.ServiceModel.Operations
{
	[DataContract]
	public class CustomerResponse : IHasResponseStatus
	{
		public CustomerResponse()
		{
			this.ResponseStatus = new ResponseStatus();
		}

		[DataMember]
		public Customer Customer { get; set; }

		[DataMember]
		public ResponseStatus ResponseStatus { get; set; }
	}

}