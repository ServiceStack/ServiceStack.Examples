using ServiceStack.ServiceInterface.ServiceModel;

namespace ServiceStack.Examples.ServiceModel.Operations
{
	public class ResetMovieDatabase
	{
	}

	public class ResetMovieDatabaseResponse
	{
		public ResetMovieDatabaseResponse()
		{
			this.ResponseStatus = new ResponseStatus();
		}

		public ResponseStatus ResponseStatus { get; set; }
	}
}