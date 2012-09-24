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
    /// <summary>
    /// Define your ServiceStack web service request (i.e. Request DTO).
    /// </summary>
    /// <remarks>The route is defined here rather than in the AppHost.</remarks>
    [Description("GET or DELETE a single movie by Id. Use POST to create a new Movie and PUT to update it")]
    [Route("/movies", "POST,PUT,PATCH,DELETE")]
    [Route("/movies/{Id}")]
    public class Movie
    {
        /// <summary>
        /// Initializes a new instance of the movie.
        /// </summary>
        public Movie()
        {
            this.Genres = new List<string>();
        }

        /// <summary>
        /// Gets or sets the id of the movie. The id will be automatically incremented when added.
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
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    public class MovieResponse
    {
        /// <summary>
        /// Gets or sets the movie.
        /// </summary>
        public Movie Movie { get; set; }
    }

    /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class MovieService : RestServiceBase<Movie>
    {
        /// <summary>
        /// Gets or sets the database factory. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
        public IDbConnectionFactory DbFactory { get; set; }

        /// <summary>
        /// GET /movies/{Id} 
        /// </summary>
        public override object OnGet(Movie movie)
        {
            return new MovieResponse {
                Movie = DbFactory.Run(dbCmd => dbCmd.GetById<Movie>(movie.Id))
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
            var newMovieId = DbFactory.Run(dbCmd => {
                dbCmd.Insert(movie);
                return dbCmd.GetLastInsertId();
            });

            var newMovie = new MovieResponse {
                Movie = DbFactory.Run(dbCmd => dbCmd.GetById<Movie>(newMovieId))
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
            DbFactory.Run(dbCmd => dbCmd.Update(movie));

            return new HttpResult()
            {
                StatusCode = HttpStatusCode.NoContent,
                Headers = {
                    { HttpHeaders.Location, this.RequestContext.AbsoluteUri.WithTrailingSlash() + movie.Id }
                }
            };
        }

        /// <summary>
        /// DELETE /movies/{Id}
        /// </summary>
        public override object OnDelete(Movie request)
        {
            DbFactory.Run(dbCmd => dbCmd.DeleteById<Movie>(request.Id));

            return new HttpResult()
            {
                StatusCode = HttpStatusCode.NoContent,
                Headers = {
                    { HttpHeaders.Location, this.RequestContext.AbsoluteUri.WithTrailingSlash() + request.Id }
                }
            };
        }
    }

    /// <summary>
    /// Define your ServiceStack web service request (i.e. Request DTO).
    /// </summary>
    /// <remarks>The route is defined here rather than in the AppHost.</remarks>
    [Description("Find movies by genre, or all movies if no genre is provided")]
    [Route("/movies", "GET, OPTIONS")]
    [Route("/movies/genres/{Genre}")]
    public class Movies
    {
        public string Genre { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    
    public class MoviesResponse
    {
        /// <summary>
        /// Gets or sets the list of movies.
        /// </summary>
        
        public List<Movie> Movies { get; set; }
    }

    /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class MoviesService : RestServiceBase<Movies>
    {
        /// <summary>
        /// Gets or sets the database factory. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
        public IDbConnectionFactory DbFactory { get; set; }

        /// <summary>
        /// GET /movies 
        /// GET /movies/genres/{Genre}
        /// </summary>
        public override object OnGet(Movies request)
        {
            return DbFactory.Run(dbCmd =>
                new MoviesResponse {
                    Movies = request.Genre.IsNullOrEmpty()
                        ? dbCmd.Select<Movie>()
                        : dbCmd.Select<Movie>("Genres LIKE {0}", "%" + request.Genre + "%")
                });
        }
    }
}