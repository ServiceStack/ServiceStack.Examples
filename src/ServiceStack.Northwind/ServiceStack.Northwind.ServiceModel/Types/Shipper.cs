using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{	
	public class Shipper
	{		
		public int Id { get; set; }		
		public string CompanyName { get; set; }		
		public string Phone { get; set; }
	}
}