namespace ServiceStack.Northwind.ServiceModel.Types
{
	using System.Runtime.Serialization;

	[DataContract]
	public class Supplier
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string CompanyName { get; set; }

		[DataMember]
		public string ContactName { get; set; }

		[DataMember]
		public string ContactTitle { get; set; }

		[DataMember]
		public string Address { get; set; }

		[DataMember]
		public string City { get; set; }

		[DataMember]
		public string Region { get; set; }

		[DataMember]
		public string PostalCode { get; set; }

		[DataMember]
		public string Country { get; set; }

		[DataMember]
		public string Phone { get; set; }

		[DataMember]
		public string Fax { get; set; }

		[DataMember]
		public string HomePage { get; set; }
	}
}