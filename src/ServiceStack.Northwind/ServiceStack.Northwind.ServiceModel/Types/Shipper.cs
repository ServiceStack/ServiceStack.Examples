namespace ServiceStack.Northwind.ServiceModel.Types
{
	using System.Runtime.Serialization;

	[DataContract]
	public class Shipper
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string CompanyName { get; set; }

		[DataMember]
		public string Phone { get; set; }
	}
}