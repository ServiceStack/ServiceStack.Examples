namespace ServiceStack.Northwind.ServiceModel.Types
{
	using System.Runtime.Serialization;

	[DataContract]
	public class OrderDetail
	{
		public string Id
		{
			get { return this.OrderId + "/" + this.ProductId; }
		}

		[DataMember]
		public int OrderId { get; set; }

		[DataMember]
		public int ProductId { get; set; }

		[DataMember]
		public decimal UnitPrice { get; set; }

		[DataMember]
		public short Quantity { get; set; }

		[DataMember]
		public double Discount { get; set; }
	}
}