namespace ServiceStack.Northwind.ServiceModel.Types
{
	using System.Runtime.Serialization;

	[DataContract]
	public class Territory
	{
		[DataMember]
		public string Id { get; set; }

		[DataMember]
		public string TerritoryDescription { get; set; }

		[DataMember]
		public int RegionId { get; set; }
	}
}