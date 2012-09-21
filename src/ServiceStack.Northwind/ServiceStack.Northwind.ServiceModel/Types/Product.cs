using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{	
	public class Product
	{		
		public int Id { get; set; }		
		public string ProductName { get; set; }		
		public int SupplierId { get; set; }		
		public int CategoryId { get; set; }		
		public string QuantityPerUnit { get; set; }		
		public decimal UnitPrice { get; set; }		
		public short UnitsInStock { get; set; }		
		public short UnitsOnOrder { get; set; }		
		public short ReorderLevel { get; set; }		
		public bool Discontinued { get; set; }
	}
}