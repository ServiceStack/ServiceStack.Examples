using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{	
	public class EmployeeTerritory
	{
		public string Id { get { return this.EmployeeId + "/" + this.TerritoryId; } }		
		public int EmployeeId { get; set; }		
		public string TerritoryId { get; set; }
	}
}