A myriad of different, pluggable options are included in Service Stack for the most popular cache providers.

# Introduction

As caching is an essential technology in the development of high-performance web services, Service Stack has a number of different caching options available that each share the same
[common client interface](https://github.com/ServiceStack/ServiceStack/blob/master/src/ServiceStack.Interfaces/CacheAccess/ICacheClient.cs)
for the following cache providers:

# Live Example and code

A live demo of the ICacheClient is available in [The ServiceStack.Northwind's example project](http://servicestack.net/ServiceStack.Northwind/). Here are some requests to cached services:

  * [/customers](http://servicestack.net/ServiceStack.Northwind/cached/customers)
  * [/customers/ALFKI](http://servicestack.net/ServiceStack.Northwind/cached/customers/ALFKI)
  * [/customers/ALFKI/orders](http://servicestack.net/ServiceStack.Northwind/cached/customers/ALFKI/orders)

Which are simply existing web services wrapped using **ICacheClient** that are contained in [CachedServices.cs](https://github.com/ServiceStack/ServiceStack.Examples/blob/master/src/ServiceStack.Northwind/ServiceStack.Northwind.ServiceInterface/CachedServices.cs)

# Cache Providers

  * [Memcached](https://github.com/ServiceStack/ServiceStack/tree/master/src/ServiceStack.CacheAccess.Memcached/) - The tried and tested most widely used cache provider.
  * [Redis](https://github.com/ServiceStack/ServiceStack.Redis/blob/master/src/ServiceStack.Redis/RedisClient.ICacheClient.cs) - A very fast key-value store that has  non-volatile persistent storage and support for rich data structures such as lists and sets.
  * [In Memory Cache](https://github.com/ServiceStack/ServiceStack/blob/master/src/ServiceStack.CacheAccess.Providers/MemoryCacheClient.cs) - Useful for single host web services and enabling unit tests to run without needing access to a cache server.
  * [FileAndCacheTextManager](https://github.com/ServiceStack/ServiceStack/blob/master/src/ServiceStack.CacheAccess.Providers/FileAndCacheTextManager.cs) - A two-tiered cache provider that caches using one of the above cache clients as well as a compressed XML or JSON serialized backup cache on the file system.

## ICacheClient the Common Interface


	//A common interface implementation that is implemented by most cache providers
	public interface ICacheClient 
		: IDisposable
	{
		//Removes the specified item from the cache.
		bool Remove(string key);

		//Removes the cache for all the keys provided.
		void RemoveAll(IEnumerable<string> keys);

		//Retrieves the specified item from the cache.
		T Get<T>(string key);

		//Increments the value of the specified key by the given amount. 
		//The operation is atomic and happens on the server.
		//A non existent value at key starts at 0
		long Increment(string key, uint amount);

		//Increments the value of the specified key by the given amount. 
		//The operation is atomic and happens on the server.
		//A non existent value at key starts at 0
		long Decrement(string key, uint amount);

		//Adds a new item into the cache at the specified cache key only if the cache is empty.
		bool Add<T>(string key, T value);

		//Sets an item into the cache at the cache key specified regardless if it already exists or not.
		bool Set<T>(string key, T value);


		//Replaces the item at the cachekey specified only if an items exists at the location already. 
		bool Replace<T>(string key, T value);

		bool Add<T>(string key, T value, DateTime expiresAt);
		bool Set<T>(string key, T value, DateTime expiresAt);
		bool Replace<T>(string key, T value, DateTime expiresAt);

		bool Add<T>(string key, T value, TimeSpan expiresIn);
		bool Set<T>(string key, T value, TimeSpan expiresIn);
		bool Replace<T>(string key, T value, TimeSpan expiresIn);


		//Invalidates all data on the cache.
		void FlushAll();

		//Retrieves multiple items from the cache. 
		//The default value of T is set for all keys that do not exist.
		IDictionary<string, T> GetAll<T>(IEnumerable<string> keys);


		//Sets multiple items to the cache. 
		void SetAll<T>(IDictionary<string, T> values);
	}


[<Wiki Home](~/framework/home)
