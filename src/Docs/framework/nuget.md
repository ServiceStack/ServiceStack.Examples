# Install ServiceStack Web Service Framework via NuGet

To make it easier for developers to get started we're now maintaining NuGet packages for ServiceStack and its components.

So if you have [NuGet](http://nuget.org) installed, the easiest way to get started is to create a new ASP.NET Web Application and install the **ServiceStack** package:

![Install-Package ServiceStack](http://mono.servicestack.net/img/nuget-servicestack.png)

This automates the following manual steps: 

* Add the ServiceStack dlls to your standard VS.NET ASP.NET Web Application 
* Register the ServiceStack handler in your Web.Config
* Configure your AppHost 
* Create a **[Hello](http://mono.servicestack.net/ServiceStack.Hello/)** web service
* Create a **[TODO](http://mono.servicestack.net/Backbone.Todos/)** RESTful web service

Although we believe this to be a popular starting point, it is not the only one as we have examples of Windows Services, Stand-alone Console Hosts, Hosting together with an existing web framework at a Custom Path - Templates available in the **/StarterTemplates** folder in the [ServiceStack.Examples project](https://github.com/ServiceStack/ServiceStack.Examples/downloads).

## NuGet ServiceStack.Text

Downloadable separately from ServiceStack itself is it's string powers. Inside [ServiceStack.Text](~/text-serializers/json-csv-jsv-serializers) are some of .NET's fastest Text Serializers:

* [JsonSerializer](http://www.servicestack.net/mythz_blog/?p=344)
* [TypeSerializer (JSV-Format)](~/text-serializers/jsv-format)
* CsvSerializer
* [T.Dump extension method](http://www.servicestack.net/mythz_blog/?p=202)
* StringExtensions - Xml/Json/Csv/Url encoding, BaseConvert, Rot13, Hex escape, etc.
* Stream, Reflection, List, DateTime, etc extensions and utils

![Install-Package ServiceStack.Text](http://mono.servicestack.net/img/nuget-servicestack.text.png)

## NuGet ServiceStack.Redis

With a hope to introduce more .NET developers to the high-performance and productive NoSQL worlds, we also include a full-featured [C# Redis client](~/redis-client/redis-client) allowing you to build [complete apps with it](http://mono.servicestack.net/RedisStackOverflow/). [Redis](http://redis.io/) is the fastest NoSQL database in the world that is capable of achieving [about 110000 SETs and 81000 GETs per second](http://redis.io/topics/benchmarks).

The C# Redis Client features:

* High-level [Redis](~/redis-client/iredisclient-api), [RedisTypedClient](~/redis-client/iredistypedclient-api) as well as [RedisNativeClient](~/redis-client/iredisnativeclient-api) for raw byte access.
* Thread-safe Basic and Pooled Redis clients managers
* [Creating Transactions and custom Atomic Operations](~/redis-client/redis-transactions)
* [Fast, efficient distributed locking with Redis](https://github.com/ServiceStack/ServiceStack.Redis/wiki/RedisLocks)
* [Publish/Subscribe messaging patterns](~/redis-client/redis-pubsub)

For .NET developers new to Redis, we invite you to check out the following tutorials:

* [Designing a NoSQL Database using Redis](~/redis-client/designing-nosql-database)
* [Painless data migrations with schema-less NoSQL datastores](~/redis-client/schemaless-nosql-migrations)

![Install-Package ServiceStack.Redis](http://mono.servicestack.net/img/nuget-servicestack.redis.png)

## NuGet ServiceStack.OrmLite

When you're developing small to medium systems and you don't need the features of an advanced heavy-weight ORM, you're sometimes better off with a fast, light-weight POCO ORM. OrmLite fills this niche, where it's just a set of lightweight extension methods on .NET's ADO.NET System.Data interfaces that non-invasively works off POCO's. 

It's primary feature over other ORMs is its auto-support for blobs where any complex property is automatically persisted in a schema-less text blob using ServiceStack.Text's fast TypeSerializer. This allows you to persist most web service requests and responses directly into an RDBMS as-is without the tedious tasks of configuring tables, ORM and mapping files.

Currently OrmLite comes in SQLite and SQL Server RDBMS's flavors and each are downloadable separately via NuGet:

![Install-Package ServiceStack.OrmLite.SqlServer](http://mono.servicestack.net/img/nuget-servicestack.ormlite.sqlserver.png)

For Sqlite 32 and 64bit embedded .NET libraries are available:

![Install-Package ServiceStack.OrmLite.Sqlite32](http://mono.servicestack.net/img/nuget-servicestack.ormlite.sqlite32.png)

![Install-Package ServiceStack.OrmLite.Sqlite64](http://mono.servicestack.net/img/nuget-servicestack.ormlite.sqlite64.png)


