using System;
using ServiceStack.Examples.ServiceModel;

namespace ServiceStack.Examples.ServiceInterface
{
	/// <summary>
	/// This is an example showing how you can take advantage of ServiceStack's built-in
	/// support for caching in your webservices.
	/// </summary>
	public class GetNorthwindCustomerOrdersCachedService : Service
	{
		public object Execute(GetNorthwindCustomerOrdersCached request)
		{
			//create a unique urn to cache these results against
			var cacheKey = IdUtils.CreateUrn<GetNorthwindCustomerOrdersCachedResponse>(request.CustomerId);

			//Removes all the cached results at this cacheKey
			if (request.RefreshCache)
				Request.RemoveFromCache(this.Cache, cacheKey);

			/*Return the most optimized result path based upon the incoming request context:
				+ If a cache of the result for the content-type already exists, then:
					- If the client accepts 'deflate' or 'gzip'd encoding the 'compressed serialized result' is returned
					- otherwise the serialized result (e.g. XML) stored in the cache is returned
			 
			    + If a cache doesn't exist then the result is created from the factory lambda and the result
					- stored at										e.g: Cache[urn:custorders:1] = result
					- the serailized contentType stored at			e.g: Cache[urn:custorders:1.xml] = result.xml
					- the compressed serialized result stored at	e.g: Cache[urn:custorders:1.xml.deflate] = result.xml.deflate
			 */
			return Request.ToOptimizedResultUsingCache(this.Cache, cacheKey, () =>
				new GetNorthwindCustomerOrdersCachedResponse {					
					CreatedDate = DateTime.UtcNow, //To determine when this cached result was created
					CustomerOrders = GetNorthwindCustomerOrdersService.GetCustomerOrders(request.CustomerId)
				});
		}
	}
}