namespace ServiceStack.Northwind.ServiceModel.Types
{
	using System.Runtime.Serialization;

	[DataContract]
	public class CustomerCustomerDemo
	{
		[DataMember]
		public string Id { get; set; }

		[DataMember]
		public string CustomerTypeId { get; set; }
	}
}