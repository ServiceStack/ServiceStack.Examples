namespace ServiceStack.Northwind.ServiceModel.Types
{
	using System.Runtime.Serialization;

	[DataContract]
	public class EmployeeTerritory
	{
		public string Id
		{
			get { return this.EmployeeId + "/" + this.TerritoryId; }
		}

		[DataMember]
		public int EmployeeId { get; set; }

		[DataMember]
		public string TerritoryId { get; set; }
	}
}