using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;

namespace RestIntro.ServiceModel
{
	[RestService("/customers")]
	[RestService("/customers/{Id}")]
	public class Customer
	{
		[AutoIncrement] //OrmLite hint
		public int Id { get; set; }
		public string Name { get; set; }
		public int Age { get; set; }
		public string Email { get; set; }
	}
}