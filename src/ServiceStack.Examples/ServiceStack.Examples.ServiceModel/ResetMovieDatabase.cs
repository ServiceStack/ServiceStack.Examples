using System.Runtime.Serialization;

namespace ServiceStack.Examples.ServiceModel
{
    [DataContract(Namespace = ExampleConfig.DefaultNamespace)]
    public class ResetMovieDatabase
	{
	}

    [DataContract(Namespace = ExampleConfig.DefaultNamespace)]
    public class ResetMovieDatabaseResponse
	{
		public ResetMovieDatabaseResponse()
		{
			this.ResponseStatus = new ResponseStatus();
		}

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }
	}
}