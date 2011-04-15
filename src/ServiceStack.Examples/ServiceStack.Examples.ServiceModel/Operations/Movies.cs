using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;
using Movie=ServiceStack.Examples.ServiceModel.Types.Movie;

namespace ServiceStack.Examples.ServiceModel.Operations
{
	public class Movies
	{
		public string Id { get; set; }

		public Movie Movie { get; set; }
	}

	public class MoviesResponse
	{
		public MoviesResponse()
		{
			this.ResponseStatus = new ResponseStatus();
			this.Movies = new List<Movie>();
		}

		public ResponseStatus ResponseStatus { get; set; }

		public List<Movie> Movies { get; set; }
	}
}