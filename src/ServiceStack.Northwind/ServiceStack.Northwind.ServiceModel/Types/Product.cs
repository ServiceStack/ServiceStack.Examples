namespace ServiceStack.Northwind.ServiceModel.Types
{
	using System.Runtime.Serialization;

	[DataContract]
	public class Product
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string ProductName { get; set; }

		[DataMember]
		public int SupplierId { get; set; }

		[DataMember]
		public int CategoryId { get; set; }

		[DataMember]
		public string QuantityPerUnit { get; set; }

		[DataMember]
		public decimal UnitPrice { get; set; }

		[DataMember]
		public short UnitsInStock { get; set; }

		[DataMember]
		public short UnitsOnOrder { get; set; }

		[DataMember]
		public short ReorderLevel { get; set; }

		[DataMember]
		public bool Discontinued { get; set; }
	}
}