using System.ComponentModel;
using ServiceStack;

namespace StarterTemplates.Common
{
    /// <summary>
    /// Define your ServiceStack web service request (i.e. the Request DTO).
    /// </summary>  
    [Description("ServiceStack's Hello World web service.")]
    [Route("/hello")]
    [Route("/hello/{Name*}")]
    public class Hello
    {		
        public string Name { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    public class HelloResponse : IHasResponseStatus
    {		
        public string Result { get; set; }		
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    /// Create your ServiceStack web service implementation.
    /// </summary>
    public class HelloService : Service
    {
        public object Any(Hello request)
        {
            return new HelloResponse { Result = "Hello, " + request.Name };
        }
    }
}