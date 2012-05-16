using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.Serialization;
using ServiceStack.Common.Extensions;
using ServiceStack.Common.Web;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;
using ServiceStack.ServiceHost;

namespace ServiceStack.MovieRest
{
	[Description("GET or DELETE a single movie by Id. Use POST to create a new Movie and PUT to update it")]
	[RestService("/movies", "POST,PUT,PATCH")]
	[RestService("/movies/{Id}")]
	public class Movie
	{
		public Movie()
		{
			this.Genres = new List<string>();
		}

		[AutoIncrement]
		public int Id { get; set; }
		public string ImdbId { get; set; }
		public string Title { get; set; }
		public decimal Rating { get; set; }
		public string Director { get; set; }
		public DateTime ReleaseDate { get; set; }
		public string TagLine { get; set; }
		public List<string> Genres { get; set; }
	}

	public class MovieResponse
	{
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
			return new MovieResponse {
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
			var newMovieId = DbFactory.Exec(dbCmd => {
				dbCmd.Insert(movie);
				return dbCmd.GetLastInsertId();
			});

			var newMovie = new MovieResponse {
				Movie = DbFactory.Exec(dbCmd => dbCmd.GetById<Movie>(newMovieId))
			};

			return new HttpResult(newMovie) {
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
			DbFactory.Exec(dbCmd => dbCmd.Update(movie));
			return null;
		}

		/// <summary>
		/// DELETE /movies/{Id}
		/// </summary>
		public override object OnDelete(Movie request)
		{
			DbFactory.Exec(dbCmd => dbCmd.DeleteById<Movie>(request.Id));
			return null;
		}
	}

	[Description("Find movies by genre, or all movies if no genre is provided")]
	[RestService("/movies", "GET, OPTIONS")]
	[RestService("/movies/genres/{Genre}")]
	public class Movies
	{
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
			return DbFactory.Exec(dbCmd =>
				new MoviesResponse {
					Movies = request.Genre.IsNullOrEmpty()
						? dbCmd.Select<Movie>()
						: dbCmd.Select<Movie>("Genres LIKE {0}", "%" + request.Genre + "%")
				});
		}
	}

}