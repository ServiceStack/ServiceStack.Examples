namespace ServiceStack.Northwind.ServiceModel.Types
{
	using System;
	using System.Runtime.Serialization;

	[DataContract]
	public class Order
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string CustomerId { get; set; }

		[DataMember]
		public int EmployeeId { get; set; }

		[DataMember]
		public DateTime? OrderDate { get; set; }

		[DataMember]
		public DateTime? RequiredDate { get; set; }

		[DataMember]
		public DateTime? ShippedDate { get; set; }

		[DataMember]
		public int? ShipVia { get; set; }

		[DataMember]
		public decimal Freight { get; set; }

		[DataMember]
		public string ShipName { get; set; }

		[DataMember]
		public string ShipAddress { get; set; }

		[DataMember]
		public string ShipCity { get; set; }

		[DataMember]
		public string ShipRegion { get; set; }

		[DataMember]
		public string ShipPostalCode { get; set; }

		[DataMember]
		public string ShipCountry { get; set; }
	}
}