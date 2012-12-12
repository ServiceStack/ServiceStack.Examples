using System.Runtime.Serialization;

namespace Northwind.ServiceModel.Types
{
	public class Category
	{		
		public int Id { get; set; }		
		public string CategoryName { get; set; }		
		public string Description { get; set; }
	}
}