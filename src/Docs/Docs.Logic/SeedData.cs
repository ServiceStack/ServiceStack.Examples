using System.Collections.Generic;

namespace Docs.Logic
{
	public class SeedData
	{
		public static List<Page> Pages = new List<Page> 
		{
			new Page("Home", "https://github.com/ServiceStack/ServiceStack/wiki/Home", "ServiceStack", new[]{"GettingStarted"}),
			new Page("Overview", "https://github.com/ServiceStack/ServiceStack/blob/master/README.md", "ServiceStack", new[]{"Overview"}),
			new Page("Release Notes", "https://github.com/ServiceStack/ServiceStack/wiki/Release-Notes", "ServiceStack", new[]{"ReleaseNotes"}),
			new Page("NuGet", "https://github.com/ServiceStack/ServiceStack/wiki/NuGet", "ServiceStack", new[]{"NuGet"}),
			new Page("JSON Report Format", "https://github.com/ServiceStack/ServiceStack/wiki/HTML5ReportFormat", "ServiceStack", new[]{"HTML5","Formats","JsonReport"}),
			new Page("Caching Options", "https://github.com/ServiceStack/ServiceStack/wiki/Caching", "ServiceStack", new[]{"Caching","Redis","Memcached"}),
			new Page("Accessing IHttpRequest", "https://github.com/ServiceStack/ServiceStack/wiki/Access-the-HTTP-Request-in-Web-Services", "ServiceStack", new[]{"API","HTTP"}),
	
			new Page("ServiceStack Examples", "https://github.com/ServiceStack/ServiceStack.Examples/blob/master/README.md", "Examples", new[]{"GettingStarted", "Overview"}),

			new Page("ServiceStack Contrib", "https://github.com/ServiceStack/ServiceStack.Contrib/blob/master/README.md", "Contrib", new[]{"Contrib", "Overview"}),

			new Page("JSON, CSV, JSV Serializers", "https://github.com/ServiceStack/ServiceStack.Text/blob/master/README.md", "Text Serializers", new[]{"JSON", "JSV", "CSV", "Text", "Overview"}),
			new Page("JSV Format", "https://github.com/ServiceStack/ServiceStack.Text/wiki/JSV-Format", "Text Serializers", new[]{"JSV", "Text"}),

			new Page("Redis Client", "https://github.com/ServiceStack/ServiceStack.Redis/blob/master/README.md", "Redis Client", new[]{"Redis"}),
			new Page("Redis Overview", "https://github.com/ServiceStack/ServiceStack.Redis/wiki/Home", "Redis Client", new[]{"Redis", "Overview"}),
			new Page("Useful Redis Links", "https://github.com/ServiceStack/ServiceStack.Redis/wiki/Useful-Redis-Links", "Redis Client", new[]{"Redis", "Overview"}),
			new Page("Redis Release Notes", "https://github.com/ServiceStack/ServiceStack.Redis/wiki/Home", "Redis Client", new[]{"Redis", "Overview", "ReleaseNotes"}),
			new Page("Designing NoSQL Database", "https://github.com/ServiceStack/ServiceStack.Redis/wiki/DesigningNoSqlDatabase", "Redis Client", new[]{"Redis", "NoSQL", "Tutorial"}),
			new Page("Schemaless NoSQL Migrations", "https://github.com/ServiceStack/ServiceStack.Redis/wiki/MigrationsUsingSchemalessNoSql", "Redis Client", new[]{"Redis", "NoSQL", "Tutorial"}),
			new Page("Distributed Locking with Redis", "https://github.com/ServiceStack/ServiceStack.Redis/wiki/MigrationsUsingSchemalessNoSql", "Redis Client", new[]{"Redis", "NoSQL", "Example", "Locks"}),
			new Page("Redis Pub/Sub", "https://github.com/ServiceStack/ServiceStack.Redis/wiki/RedisPubSub", "Redis Client", new[]{"Redis", "NoSQL", "Example", "PubSub"}),
			new Page("Redis Transactions", "https://github.com/ServiceStack/ServiceStack.Redis/wiki/RedisTransactions", "Redis Client", new[]{"Redis", "NoSQL", "Example", "Transactions"}),
			new Page("IRedisClient API", "https://github.com/ServiceStack/ServiceStack.Redis/wiki/IRedisClient", "Redis Client", new[]{"Redis", "API", "RedisClient"}),
			new Page("IRedisTypedClient API", "https://github.com/ServiceStack/ServiceStack.Redis/wiki/IRedisTypedClient", "Redis Client", new[]{"Redis", "API", "RedisClient"}),
			new Page("IRedisNativeClient API", "https://github.com/ServiceStack/ServiceStack.Redis/wiki/IRedisNativeClient", "Redis Client", new[]{"Redis", "API", "RedisClient"}),
			new Page("IRedisTransaction API", "https://github.com/ServiceStack/ServiceStack.Redis/wiki/IRedisTransaction", "Redis Client", new[]{"Redis", "API", "Transactions"}),
			
			new Page("Redis Admin UI Overview", "https://github.com/ServiceStack/ServiceStack.RedisWebServices/blob/master/README.md", "Redis Admin UI", new[]{"Redis", "RedisAdminUI", "Overview"}),

			new Page("OrmLite Overview", "https://github.com/ServiceStack/ServiceStack.OrmLite/blob/master/README.md", "OrmLite", new[]{"OrmLite", "SqlServer", "Sqlite"}),
			
			new Page("Logging Overview", "https://github.com/ServiceStack/ServiceStack.OrmLite/blob/master/README.md", "Logging", new[]{"Logging", "Log4Net", "EventLog", "Nlog"}),
		};
	}
}