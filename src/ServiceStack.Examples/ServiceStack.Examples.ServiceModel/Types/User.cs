using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace ServiceStack.Examples.ServiceModel.Types
{
	[DataContract(Namespace = ExampleConfig.DefaultNamespace)]
	public class User
	{
		[AutoIncrement]		
		public int Id { get; set; }		
		public string UserName { get; set; }		
		public string Email { get; set; }		
		public string Password { get; set; }		
		public Guid GlobalId { get; set; }
	}
}