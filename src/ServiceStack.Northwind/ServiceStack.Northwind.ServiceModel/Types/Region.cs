using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{
	[DataContract]
	public class Region
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string RegionDescription { get; set; }
	}
}