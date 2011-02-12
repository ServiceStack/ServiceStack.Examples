using System.Runtime.Serialization;

namespace ServiceStack.Northwind.ServiceModel.Types
{
	[DataContract]
	public class Category
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string CategoryName { get; set; }
		[DataMember]
		public string Description { get; set; }
	}
}