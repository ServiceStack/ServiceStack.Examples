using System;
using System.Runtime.Serialization;
using ServiceStack.Examples.ServiceModel.Types;

namespace ServiceStack.Examples.ServiceModel
{
    [DataContract(Namespace = ExampleConfig.DefaultNamespace)]
    public class GetNorthwindCustomerOrdersCached
	{
        [DataMember]
        public bool RefreshCache { get; set; }

        [DataMember]
        public string CustomerId { get; set; }
	}

    [DataContract(Namespace = ExampleConfig.DefaultNamespace)]
    public class GetNorthwindCustomerOrdersCachedResponse
	{
		public GetNorthwindCustomerOrdersCachedResponse()
		{
			this.ResponseStatus = new ResponseStatus();
		}

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public CustomerOrders CustomerOrders { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }
	}
}