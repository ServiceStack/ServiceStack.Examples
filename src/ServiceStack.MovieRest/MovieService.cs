using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.Serialization;
using ServiceStack.Common.Extensions;
using ServiceStack.Common.Web;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;

namespace ServiceStack.MovieRest
{
	[Description("GET or DELETE a single movie by Id. Use POST to create a new Movie and PUT to update it")]
	[RestService("/movies", "POST,PUT")]
	[RestService("/movies/{Id}")]
	[DataContract]
	public class Movie
	{
		public Movie()
		{
			this.Genres = new List<string>();
		}

		[DataMember] [AutoIncrement]
		public int Id { get; set; }

		[DataMember]
		public string ImdbId { get; set; }

		[DataMember]
		public string Title { get; set; }

		[DataMember]
		public decimal Rating { get; set; }

		[DataMember]
		public string Director { get; set; }

		[DataMember]
		public DateTime ReleaseDate { get; set; }

		[DataMember]
		public string TagLine { get; set; }

		[DataMember]
		public List<string> Genres { get; set; }
	}

	[DataContract]
	public class MovieResponse
	{
		[DataMember]
		public Movie Movie { get; set; }
	}

	public class MovieService : RestServiceBase<Movie>
	{
		public IDbConnectionFactory DbFactory { get; set; }

		/// <summary>
		/// GET /movies/{Id} 
		/// </summary>
		public override object OnGet(Movie movie)
		{
			return new MovieResponse
			{
				Movie = DbFactory.Exec(dbCmd => dbCmd.GetById<Movie>(movie.Id))
			};
		}

		/// <summary>
		/// POST /movies
		/// 
		/// returns HTTP Response => 
		/// 	201 Created
		///     Location: http://localhost/ServiceStack.MovieRest/movies/{newMovieId}
		/// 	
		/// 	{newMovie DTO in [xml|json|jsv|etc]}
		/// 
		/// </summary>
		public override object OnPost(Movie movie)
		{
			var newMovieId = DbFactory.Exec(dbCmd =>
			{
				dbCmd.Insert(movie);
				return dbCmd.GetLastInsertId();
			});

			var newMovie = new MovieResponse {
				Movie = DbFactory.Exec(dbCmd => dbCmd.GetById<Movie>(newMovieId))
			};

			return new HttpResult(newMovie)
			{
				StatusCode = HttpStatusCode.Created,
				Headers = {
					{ HttpHeaders.Location, this.RequestContext.AbsoluteUri.WithTrailingSlash() + newMovieId }
				}
			};
		}

		/// <summary>
		/// PUT /movies/{id}
		/// </summary>
		public override object OnPut(Movie movie)
		{
			DbFactory.Exec(dbCmd => dbCmd.Save(movie));
			return new MovieResponse();
		}

		/// <summary>
		/// DELETE /movies/{Id}
		/// </summary>
		public override object OnDelete(Movie request)
		{
			DbFactory.Exec(dbCmd => dbCmd.DeleteById<Movie>(request.Id));
			return new MovieResponse();
		}
	}


	[DataContract]
	[Description("Find movies by genre, or all movies if no genre is provided")]
	[RestService("/movies", "GET")]
	[RestService("/movies/genres/{Genre}")]
	public class Movies
	{
		[DataMember]
		public string Genre { get; set; }
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
		/// GET /movies/genres/{Genre}
		/// </summary>
		public override object OnGet(Movies request)
		{
			return new MoviesResponse
			{
				Movies = request.Genre.IsNullOrEmpty()
					? DbFactory.Exec(dbCmd => dbCmd.Select<Movie>())
					: DbFactory.Exec(dbCmd => dbCmd.Select<Movie>("Genres LIKE {0}", "%" + request.Genre + "%"))
			};
		}
	}

}