namespace ServiceStack.Northwind.ServiceModel.Types
{
	using System.Collections.Generic;
	using System.Runtime.Serialization;

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