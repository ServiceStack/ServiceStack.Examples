namespace ServiceStack.MovieRest
{
	using System;
	using System.Collections.Generic;
	using ServiceStack.OrmLite;
	using ServiceStack.ServiceHost;
	using ServiceStack.ServiceInterface;

	/// <summary>
	///     Define your ServiceStack web service request (i.e. Request DTO).
	/// </summary>
	/// <remarks>The route is defined here rather than in the AppHost.</remarks>
	[Route("/reset-movies", "GET,POST")]
	[Api("Resets the database back to the original Top 5 movies.")]
	public class ResetMovies : IReturn<ResetMoviesResponse>
	{
	}

	/// <summary>
	///     Define your ServiceStack web service response (i.e. Response DTO).
	/// </summary>
	public class ResetMoviesResponse
	{
	}

	/// <summary>
	///     Create your ServiceStack rest-ful web service implementation.
	/// </summary>
	public class ResetMoviesService : Service
	{
		public static List<Movie> Top5Movies = new List<Movie>
			                                       {
				                                       new Movie
					                                       {
						                                       ImdbId = "tt1375666",
						                                       Title = "Inception",
						                                       Rating = 9.2m,
						                                       Director = "Christopher Nolan",
						                                       ReleaseDate = new DateTime(2010, 7, 16),
						                                       TagLine = "Your mind is the scene of the crime",
						                                       Genres = new List<string> {"Action", "Thriller", "Sci-Fi"},
					                                       },
				                                       new Movie
					                                       {
						                                       ImdbId = "tt0111161",
						                                       Title = "The Shawshank Redemption",
						                                       Rating = 9.2m,
						                                       Director = "Frank Darabont",
						                                       ReleaseDate = new DateTime(1995, 2, 17),
						                                       TagLine = "Fear can hold you prisoner. Hope can set you free.",
						                                       Genres = new List<string> {"Crime", "Drama"},
					                                       },
				                                       new Movie
					                                       {
						                                       ImdbId = "tt0071562",
						                                       Title = "The Godfather: Part II",
						                                       Rating = 9.0m,
						                                       Director = "Francis Ford Coppola",
						                                       ReleaseDate = new DateTime(1974, 12, 20),
						                                       Genres = new List<string> {"Crime", "Drama", "Thriller"},
					                                       },
				                                       new Movie
					                                       {
						                                       ImdbId = "tt0068646",
						                                       Title = "The Godfather",
						                                       Rating = 9.2m,
						                                       Director = "Francis Ford Coppola",
						                                       ReleaseDate = new DateTime(1972, 3, 24),
						                                       TagLine = "An offer you can't refuse.",
						                                       Genres = new List<string> {"Crime", "Drama", "Thriller"},
					                                       },
				                                       new Movie
					                                       {
						                                       ImdbId = "tt0060196",
						                                       Title = "The Good, the Bad and the Ugly",
						                                       Rating = 9.0m,
						                                       Director = "Sergio Leone",
						                                       ReleaseDate = new DateTime(1967, 12, 29),
						                                       TagLine =
							                                       "They formed an alliance of hate to steal a fortune in dead man's gold",
						                                       Genres = new List<string> {"Adventure", "Western"},
					                                       },
			                                       };

		public object Any(ResetMovies request)
		{
			//Executes the specified delegate against the configured database.
			Db.DropAndCreateTable<Movie>();
			Db.SaveAll(Top5Movies);

			return new ResetMoviesResponse();
		}
	}
}