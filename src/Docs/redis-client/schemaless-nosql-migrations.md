This page runs through a typical example to show how painless typical data migrations can be when using Redis and other NoSQL schema-less data stores.

#### All Redis Blog application pages 
  * [Designing a NoSQL Database using Redis](~/redis-client/designing-nosql-database)
  * Painless data migrations using Redis and other schema-less NoSQL datastores

# Painless data migrations with schema-less NoSQL datastores and Redis 

Developing *new* greenfield database systems utilizing a RDBMS back-end is mostly a trouble-free experience. Before the system is live, you're able to easily modify a schema by nuking the entire application database, and re-creating it with automated DDL scripts that will create and populate it with test data that fits your new schema. 

The real troubles in your IT life happens after your first deployment and your system goes live. At that point you no longer have the option to nuke the database and re-create it from scratch. If you're lucky, you have a script in place that can automatically infer the required DDL statements to migrate from your old schema to your new one. However any significant changes to your schema are likely to involve late nights, downtime, and a non-trivial amount of effort to ensure a successful migration to the new db schema.

This process is much less painful with schema-less data stores. In fact in most cases when you're just adding and removing fields it doesn't exist at all. By not having your datastore understand intrinsic details of your schema, it means it's no longer an infrastructure-level issue and can easily be handled by application logic if needed.

Being maintenance-free, schema-less and non-intrusive are fundamental design qualities baked into Redis and its operations. For example querying a list of recent BlogPosts returns the same result for an *empty list* as it would in an *empty Redis database* - 0 results. As values in Redis are binary-safe strings you're able to store anything you want in them and most importantly by extension this means that all Redis operations can support all of your application types without needing an 'intermediate language' like DDL to provide a rigid schema of what to expect. Without any prior initialization your code can talk directly to a Redis datastore naturally as if it was an in-memory collection.

To illustrate what can be achieved in practice, I will run through two different strategies of handling schema changes.

  * The do-nothing approach - where adding, removing fields and non-destructive change of field types are automatically handled.
  * Using a custom translation - using application level logic to customize the translation between the old and new types.

The full runnable source code for these example are [available here](https://github.com/ServiceStack/ServiceStack.Redis/blob/master/tests/ServiceStack.Redis.Tests/Examples/BestPractice/BlogPostMigrations.cs).

## Example Code 
To demonstrate a typical migration scenario, I'm using the `BlogPost` type defined on the previous page to project it to a fundamentally different `New.BlogPost` type. The full definition of the old and new types are shown below:

### The old schema 

	public class BlogPost
	{
		public BlogPost()
		{
			this.Categories = new List<string>();
			this.Tags = new List<string>();
			this.Comments = new List<BlogPostComment>();
		}

		public int Id { get; set; }
		public int BlogId { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public List<string> Categories { get; set; }
		public List<string> Tags { get; set; }
		public List<BlogPostComment> Comments { get; set; }
	}

	public class BlogPostComment
	{
		public string Content { get; set; }
		public DateTime CreatedDate { get; set; }
	}

### The new schema 
The 'new version' contains most changes you are likely to encounter in normal app development:
  * Added, removed and renamed fields
  * Non-destructive change of `int` into `long` and `double` fields
  * Changed Tag collection type from a `List` to a `HashSet`
  * Changed a strongly-typed `BlogPostComment` type into a loosely-typed string `Dictionary`
  * Introduced a new `enum` type
  * Added a nullable calculated field

	public class BlogPost
	{
		public BlogPost()
		{
			this.Labels = new List<string>();
			this.Tags = new HashSet<string>();
			this.Comments = new List<Dictionary<string, string>>();
		}

		//Changed int types to both a long and a double type
		public long Id { get; set; }
		public double BlogId { get; set; }

		//Added new field
		public BlogPostType PostType { get; set; }

		public string Title { get; set; }
		public string Content { get; set; }

		//Renamed from 'Categories' to 'Labels'
		public List<string> Labels { get; set; }

		//Changed from List to a HashSet
		public HashSet<string> Tags { get; set; }

		//Changed from List of strongly-typed 'BlogPostComment' to loosely-typed string map
		public List<Dictionary<string, string>> Comments { get; set; }

		//Added pointless calculated field
		public int? NoOfComments { get; set; }
	}

	public enum BlogPostType
	{
		None,
		Article,
		Summary,
	}


### 1. The do-nothing approach - using the old data with the new types 
Although hard to believe, with no extra effort you can just pretend *no change was actually done* and freely access new types looking at old data.
This is possible when there is non-destructive changes (i.e. no loss of information) with new field types. 
The example below uses the repository from the previous example to populate Redis with test data from the old types. Just as if nothing happened you can read the old data using the new type:


	var repository = new BlogRepository(redisClient);

	//Populate the datastore with the old schema from the 'BlogPostBestPractice'
	BlogPostBestPractice.InsertTestData(repository);

	//Create a typed-client based on the new schema
	using (var redisBlogPosts = redisClient.GetTypedClient<New.BlogPost>())
	{
		//Automatically retrieve blog posts
		IList<New.BlogPost> allBlogPosts = redisBlogPosts.GetAll();

		//Print out the data in the list of 'New.BlogPost' populated from old 'BlogPost' type
		Console.WriteLine(allBlogPosts.Dump());
		/*Output:
		[
			{
				Id: 3,
				BlogId: 2,
				PostType: None,
				Title: Redis,
				Labels: [],
				Tags: 
				[
					Redis,
					NoSQL,
					Scalability,
					Performance
				],
				Comments: 
				[
					{
						Content: First Comment!,
						CreatedDate: 2010-04-28T21:42:03.9484725Z
					}
				]
			},
			{
				Id: 4,
				BlogId: 2,
				PostType: None,
				Title: Couch Db,
				Labels: [],
				Tags: 
				[
					CouchDb,
					NoSQL,
					JSON
				],
				Comments: 
				[
					{
						Content: First Comment!,
						CreatedDate: 2010-04-28T21:42:03.9484725Z
					}
				]
			},
			{
				Id: 1,
				BlogId: 1,
				PostType: None,
				Title: RavenDB,
				Labels: [],
				Tags: 
				[
					Raven,
					NoSQL,
					JSON,
					.NET
				],
				Comments: 
				[
					{
						Content: First Comment!,
						CreatedDate: 2010-04-28T21:42:03.9004697Z
					},
					{
						Content: Second Comment!,
						CreatedDate: 2010-04-28T21:42:03.9004697Z
					}
				]
			},
			{
				Id: 2,
				BlogId: 1,
				PostType: None,
				Title: Cassandra,
				Labels: [],
				Tags: 
				[
					Cassandra,
					NoSQL,
					Scalability,
					Hashing
				],
				Comments: 
				[
					{
						Content: First Comment!,
						CreatedDate: 2010-04-28T21:42:03.9004697Z
					}
				]
			}
		]

		 */
	}


### 2. Using a custom translation to migrate data using application logic 

Some drawbacks with the above 'do-nothing' approach is that you will lose the data of 'renamed fields'. 
There will also be times when you want the newly migrated data to have specific values that are different from the .NET built-in defaults.
When you want more control over the migration of your old data, adding a custom translation is a trivial exercise when you can do it natively in code:


	var repository = new BlogRepository(redisClient);

	//Populate the datastore with the old schema from the 'BlogPostBestPractice'
	BlogPostBestPractice.InsertTestData(repository);

	//Create a typed-client based on the new schema
	using (var redisBlogPosts = redisClient.GetTypedClient<BlogPost>())
	using (var redisNewBlogPosts = redisClient.GetTypedClient<New.BlogPost>())
	{
		//Automatically retrieve blog posts
		IList<BlogPost> oldBlogPosts = redisBlogPosts.GetAll();

		//Write a custom translation layer to migrate to the new schema
		var migratedBlogPosts = oldBlogPosts.ConvertAll(old => new New.BlogPost
		{
			Id = old.Id,
			BlogId = old.BlogId,
			Title = old.Title,
			Content = old.Content,
			Labels = old.Categories, //populate with data from renamed field
			PostType = New.BlogPostType.Article, //select non-default enum value
			Tags = old.Tags,
			Comments = old.Comments.ConvertAll(x => new Dictionary<string, string> 
				{ { "Content", x.Content }, { "CreatedDate", x.CreatedDate.ToString() }, }),
			NoOfComments = old.Comments.Count, //populate using logic from old data
		});

		//Persist the new migrated blogposts 
		redisNewBlogPosts.StoreAll(migratedBlogPosts);

		//Read out the newly stored blogposts
		var refreshedNewBlogPosts = redisNewBlogPosts.GetAll();
		//Note: data renamed fields are successfully migrated to the new schema
		Console.WriteLine(refreshedNewBlogPosts.Dump());
		/*
		[
			{
				Id: 3,
				BlogId: 2,
				PostType: Article,
				Title: Redis,
				Labels: 
				[
					NoSQL,
					Cache
				],
				Tags: 
				[
					Redis,
					NoSQL,
					Scalability,
					Performance
				],
				Comments: 
				[
					{
						Content: First Comment!,
						CreatedDate: 28/04/2010 22:58:35
					}
				],
				NoOfComments: 1
			},
			{
				Id: 4,
				BlogId: 2,
				PostType: Article,
				Title: Couch Db,
				Labels: 
				[
					NoSQL,
					DocumentDB
				],
				Tags: 
				[
					CouchDb,
					NoSQL,
					JSON
				],
				Comments: 
				[
					{
						Content: First Comment!,
						CreatedDate: 28/04/2010 22:58:35
					}
				],
				NoOfComments: 1
			},
			{
				Id: 1,
				BlogId: 1,
				PostType: Article,
				Title: RavenDB,
				Labels: 
				[
					NoSQL,
					DocumentDB
				],
				Tags: 
				[
					Raven,
					NoSQL,
					JSON,
					.NET
				],
				Comments: 
				[
					{
						Content: First Comment!,
						CreatedDate: 28/04/2010 22:58:35
					},
					{
						Content: Second Comment!,
						CreatedDate: 28/04/2010 22:58:35
					}
				],
				NoOfComments: 2
			},
			{
				Id: 2,
				BlogId: 1,
				PostType: Article,
				Title: Cassandra,
				Labels: 
				[
					NoSQL,
					Cluster
				],
				Tags: 
				[
					Cassandra,
					NoSQL,
					Scalability,
					Hashing
				],
				Comments: 
				[
					{
						Content: First Comment!,
						CreatedDate: 28/04/2010 22:58:35
					}
				],
				NoOfComments: 1
			}
		]

		 */
	}


The end result is a datastore filled with new data populated in exactly the way you want it - ready to serve features of your new application.
In contrast, attempting the above in a typical RDBMS solution without any downtime is effectively a feat of magic that is rewardable by 999 [Stack Overflow](http://stackoverflow.com) points and a personal condolence from its grand chancellor [@JonSkeet](http://twitter.com/jonskeet) :) 

I hope this clearly illustrates differences between the two technologies. In practice you'll be amazed by the productivity gains made possible when you don't have to model your application to fit around an ORM and an RDBMS and can save objects like it was memory.

It's always a good idea to expose yourself to new technologies so if you haven't already done so, I invite you to get started developing with Redis today to see the benefits for yourself. To get started all you need is an instance of 
[the redis-server](http://code.google.com/p/servicestack/wiki/RedisWindowsDownload) (no configuration required, just unzip and run) and the dependency-free 
[ServiceStack's C# Redis Client](~/redis-client/redis-client) and you're ready to go!
