using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{	
	public class OrderDetail
	{
		public string Id { get { return this.OrderId + "/" + this.ProductId; } }		
		public int OrderId { get; set; }		
		public int ProductId { get; set; }		
		public decimal UnitPrice { get; set; }		
		public short Quantity { get; set; }		
		public double Discount { get; set; }
	}
}