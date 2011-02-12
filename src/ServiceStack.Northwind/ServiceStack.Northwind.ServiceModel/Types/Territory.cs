using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{
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