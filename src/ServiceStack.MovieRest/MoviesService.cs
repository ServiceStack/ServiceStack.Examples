using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Common.Extensions;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace ServiceStack.MovieRest
{
	[DataContract]
	[RestService("/movies", "GET")]
	[RestService("/movies/genres/{Genre}")]
	public class Movies
	{
		[DataMember]
		public string Genre { get; set; }

		[DataMember]
		public Movie Movie { get; set; }
	}

	[DataContract]
	public class MoviesResponse
	{
		[DataMember]
		public List<Movie> Movies { get; set; }
	}


	public class MoviesService : RestServiceBase<Movies>
	{
		public IDbConnectionFactory DbFactory { get; set; }

		/// <summary>
		/// GET /movies 
		/// GET /movies/genres/{Category}
		/// </summary>
		public override object Get(Movies request)
		{
			return new MoviesResponse
			{
				Movies = request.Genre.IsNullOrEmpty()
					? DbFactory.Exec(dbCmd => dbCmd.Select<Movie>())
					: DbFactory.Exec(dbCmd => dbCmd.Select<Movie>("Genre LIKE {0}", "%" + request.Genre + "%"))
			};
		}
	}

}