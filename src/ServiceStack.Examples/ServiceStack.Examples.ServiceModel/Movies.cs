using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Examples.ServiceModel.Types;

namespace ServiceStack.Examples.ServiceModel
{
    [DataContract(Namespace = ExampleConfig.DefaultNamespace)]
    public class Movies
	{
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public Movie Movie { get; set; }
	}

    [DataContract(Namespace = ExampleConfig.DefaultNamespace)]
    public class MoviesResponse
	{
		public MoviesResponse()
		{
			this.ResponseStatus = new ResponseStatus();
			this.Movies = new List<Movie>();
		}

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }

        [DataMember]
        public List<Movie> Movies { get; set; }
	}
}