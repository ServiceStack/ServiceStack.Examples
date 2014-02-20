using System.Runtime.Serialization;

namespace ServiceStack.MovieRest
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using DataAnnotations;
    using OrmLite;

    /// <summary>
    ///     Define your ServiceStack web service request (i.e. Request DTO).
    /// </summary>
    /// <remarks>The route is defined here rather than in the AppHost.</remarks>
    [Api("GET or DELETE a single movie by Id. Use POST to create a new Movie and PUT to update it")]
    [Route("/movies", "POST,PUT,PATCH,DELETE")]
    [Route("/movies/{Id}")]
    public class Movie : IReturn<MovieResponse>
    {
        /// <summary>
        ///     Initializes a new instance of the movie.
        /// </summary>
        public Movie()
        {
            this.Genres = new List<string>();
        }

        /// <summary>
        ///     Gets or sets the id of the movie. The id will be automatically incremented when added.
        /// </summary>
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

    /// <summary>
    ///     Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    public class MovieResponse
    {
        /// <summary>
        ///     Gets or sets the movie.
        /// </summary>
        public Movie Movie { get; set; }
    }

    /// <summary>
    ///     Define your ServiceStack web service request (i.e. Request DTO).
    /// </summary>
    /// <remarks>The route is defined here rather than in the AppHost.</remarks>
    [Api("Find movies by genre, or all movies if no genre is provided")]
    [Route("/movies", "GET, OPTIONS")]
    [Route("/movies/genres/{Genre}")]
    public class Movies : IReturn<MoviesResponse>
    {
        public string Genre { get; set; }
    }

    /// <summary>
    ///     Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    [DataContract]
    public class MoviesResponse
    {
        /// <summary>
        ///     Gets or sets the list of movies.
        /// </summary>
        [DataMember]
        public List<Movie> Movies { get; set; }
    }

    /// <summary>
    ///     Create your ServiceStack restful web service implementation.
    /// </summary>
    public class MovieService : Service
    {
        /// <summary>
        ///     GET /movies
        ///     GET /movies/genres/{Genre}
        /// </summary>
        public object Get(Movies request)
        {
            return new MoviesResponse {
                Movies = request.Genre.IsNullOrEmpty()
                    ? Db.Select<Movie>()
                    : Db.Select<Movie>("Genres LIKE {0}", "%{0}%".Fmt(request.Genre))
            };
        }

        /// <summary>
        ///     GET /movies/{Id}
        /// </summary>
        public MovieResponse Get(Movie movie)
        {
            return new MovieResponse
            {
                Movie = Db.SingleById<Movie>(movie.Id),
            };
        }

        /// <summary>
        ///     POST /movies
        ///     returns HTTP Response =>
        ///     201 Created
        ///     Location: http://localhost/ServiceStack.MovieRest/movies/{newMovieId}
        ///     {newMovie DTO in [xml|json|jsv|etc]}
        /// </summary>
        public object Post(Movie movie)
        {
            Db.Save(movie);
            var newMovieId = movie.Id;

            var newMovie = new MovieResponse
            {
                Movie = Db.SingleById<Movie>(newMovieId),
            };

            return new HttpResult(newMovie)
            {
                StatusCode = HttpStatusCode.Created,
                Headers = {
					{HttpHeaders.Location, base.Request.AbsoluteUri.CombineWith(newMovieId.ToString())}
				}
            };
        }

        /// <summary>
        ///     PUT /movies/{id}
        /// </summary>
        public object Put(Movie movie)
        {
            Db.Update(movie);

            return new HttpResult
            {
                StatusCode = HttpStatusCode.NoContent,
                Headers =
					{
						{HttpHeaders.Location, this.Request.AbsoluteUri.CombineWith(movie.Id.ToString())}
					}
            };
        }

        /// <summary>
        ///     DELETE /movies/{Id}
        /// </summary>
        public object Delete(Movie request)
        {
            Db.DeleteById<Movie>(request.Id);

            return new HttpResult
            {
                StatusCode = HttpStatusCode.NoContent,
                Headers = {
					{HttpHeaders.Location, this.Request.AbsoluteUri.CombineWith(request.Id.ToString())}
				}
            };
        }
    }
}