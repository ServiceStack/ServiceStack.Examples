using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Common.Extensions;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace ServiceStack.MovieRest
{
	[DataContract]
	[RestService("/movies")]
	[RestService("/movies/category/{Category}")]
	public class Movies
	{
		[DataMember]
		public string Category { get; set; }

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
		/// GET /movies/category/{Category}
		/// </summary>
		public override object Get(Movies request)
		{
			return new MoviesResponse
			{
				Movies = request.Category.IsNullOrEmpty()
					? DbFactory.Exec(dbCmd => dbCmd.Select<Movie>())
					: DbFactory.Exec(dbCmd => dbCmd.Select<Movie>("Category = {0}", request.Category))
			};
		}
	}

}