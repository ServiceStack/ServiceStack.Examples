using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{
	[DataContract]
	public class EmployeeTerritory
	{
		public string Id { get { return this.EmployeeId + "/" + this.TerritoryId; } }
		[DataMember]
		public int EmployeeId { get; set; }
		[DataMember]
		public string TerritoryId { get; set; }
	}
}