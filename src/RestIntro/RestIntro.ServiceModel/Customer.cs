using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;

namespace RestIntro.ServiceModel
{
    /// <summary>
    /// Define your ServiceStack web service request (i.e. Request DTO).
    /// </summary>
    /// <remarks>The route is defined here rather than in the AppHost.</remarks>
    [Route("/customers")]
    [Route("/customers/{Id}")]
    public class Customer
    {
        [AutoIncrement] //OrmLite hint
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
    }
}