using System.Runtime.Serialization;

namespace Northwind.ServiceModel.Types
{	
	public class Territory
	{		
		public string Id { get; set; }		
		public string TerritoryDescription { get; set; }		
		public int RegionId { get; set; }
	}
}