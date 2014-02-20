using ServiceStack;

namespace RedisStackOverflow.ServiceModel
{
    /// <summary>
    /// Define your ServiceStack web service request (i.e. the Request DTO).
    /// </summary> 
    [Route("/reset")]
    public class Reset { }

    /// <summary>
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    public class ResetResponse : IHasResponseStatus
    {
        public ResetResponse()
        {
            //Comment this out if you wish to receive the response status.
            this.ResponseStatus = new ResponseStatus();
        }

        /// <summary>
        /// Gets or sets the ResponseStatus. The built-in Ioc used with ServiceStack autowires this property with service exceptions.
        /// </summary>
        public ResponseStatus ResponseStatus { get; set; }
    }

}