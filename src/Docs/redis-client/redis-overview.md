# Welcome to the ServiceStack.Redis C# Client wiki!

This index hopes to provide a good starting page to find information about the [ServiceStack's Open Source C# Redis Client](~/redis-client/redis-client).

The Redis Client runs on Windows with .NET and Linux with [Mono](http://www.mono-project.com). 
Included as part of ServiceStack is:

* [IRedisNativeClient](~/redis-client/iredisnativeclient-api) -A low-level API that provides raw byte access to a Redis server. Each method maps to 1:1 to a Redis operation of the same name.
* **[IRedisClient](~/redis-client/iredisclient-api)** - A friendly, more descriptive API implemented by the ServiceStack.Redis client that provides access to key values as strings (or collection of strings for Redis lists and sets).
* **[IRedisTypedClient](~/redis-client/iredistypedclient-api)** - A high-level 'strongly-typed' API available on Service Stack's C# Redis Client to make all Redis Value operations to apply against any c# type. Where all complex types are transparently serialized to JSON using [ServiceStack JsonSerializer](~/text-serializers/json-csv-jsv-serializers) - [The fastest JSON Serializer for .NET](http://www.servicestack.net/mythz_blog/?p=344).
* Thread-safe `BasicRedisClientManager` and `PooledRedisClientManager` connection pooling implementations which plugs nicely in your local IOC and is useful when talking to Redis inside an ASP.NET application or Windows Service.

# Download

Links the the latest packages are [available on the home page](~/redis-client/redis-client).
Alternatively view all releases on the [projects downloads page](https://github.com/ServiceStack/ServiceStack.Redis/downloads).

## Getting Started

If you are new to Redis (or NoSQL in general) I recommend the following resources:

* [Useful Links about Redis](~/redis-client/useful-redis-links)
* [Designing a Simple Blog application with Redis](~/redis-client/designing-nosql-database)
  - To get a flavour of how to use the C# Client view the [above example source code](https://github.com/ServiceStack/ServiceStack.Redis/blob/master/tests/ServiceStack.Redis.Tests/Examples/BlogPostExample.cs) as well as the re-factored, [Best Practices / Repository approach here](https://github.com/ServiceStack/ServiceStack.Redis/blob/master/tests/ServiceStack.Redis.Tests/Examples/BestPractice/BlogPostBestPractice.cs).
* [Painless data migrations with schema-less NoSQL datastores and Redis](~/redis-client/schemaless-nosql-migrations) - Which demonstrates how flexible and resilient the JsonSerializer is with the Schema-less solution used with Redis.

## Redis Admin UI

Spawned from deep within the fires of Unix, for the longest time the only interface access to your Redis DataStore was through the highly-functional but still command-line only **redis-cli** command line utility.

Also developed in conjunction with the C# Redis Client, to help with visualizing your data in Redis is the [Redis Admin UI](http://www.servicestack.net/mythz_blog/?p=381). It was specifically designed to take advantages of the conventions of the C# Redis Client to provide a fast, functional view into your data. Like all of ServiceStack it runs on .NET and Mono with the [public demo hosted on CentOS/Nginx/Mono](http://www.servicestack.net/RedisAdminUI/AjaxClient/).

The download and source code for the Redis Admin UI is maintained in the [ServiceStack.RedisWebServices project](~/redis-admin-ui/redis-admin-ui-overview).

## Ajax Web Services for Redis

The Redis Admin UI is actually a pure Ajax web application (i.e. a pure static JavaScript file). In order to be able to develop an Ajax application that talks to Redis (which is a highly optimized, binary-safe tcp protocol) we must make the Redis operations available via Ajax Web Services. Which happens to be exactly what the [ServiceStack.RedisWebServices](~/redis-admin-ui/redis-admin-ui-overview) does. It takes advantage of ServiceStack to provide XML, JSON, JSV, SOAP 1.1/1.2 for all of Redis operations. A [list of all the operations available](http://www.servicestack.net/RedisAdminUI/Public/Metadata) can be seen on the public demo.

Effectively [ServiceStack.RedisWebServices](~/redis-admin-ui/redis-admin-ui-overview) gives Redis CouchDB-like powers where the Ajax Web Services layer allows websites to talk directly to Redis without any custom middle-tier just like the Redis Admin UI :)

# Advanced C# Redis Client features

After you have familiarized yourself with the basics of the Redis Client here are a few useful resources to explore the Advanced features and capabilities of Redis:

* How to create [Transactions / Atomic Operations](~/redis-client/redis-transactions) with Redis using the [IRedisTransaction](~/redis-client/iredistransaction-api) and [IRedisTypedTransaction](https://github.com/ServiceStack/ServiceStack.Redis/wiki/IRedisTypedTransaction) APIs
* [Fast, efficient distributed locking with Redis](https://github.com/ServiceStack/ServiceStack.Redis/wiki/RedisLocks)
* [Pub/Sub Messaging with Redis](~/redis-client/redis-pubsub)





