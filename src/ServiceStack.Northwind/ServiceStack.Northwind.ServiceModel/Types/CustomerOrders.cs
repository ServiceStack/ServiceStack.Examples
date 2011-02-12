using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{
	[DataContract]
	public class CustomerOrder
	{
		public CustomerOrder()
		{
			this.OrderDetails = new List<OrderDetail>();
		}

		[DataMember]
		public Order Order { get; set; }

		[DataMember]
		public List<OrderDetail> OrderDetails { get; set; }
	}
}