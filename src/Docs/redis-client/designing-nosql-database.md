This page illustrates a good solution on how to design a simple Blog application with [Redis](http://code.google.com/p/redis/) using advanced features of 
[ServiceStack's C# Redis Client](~/redis-client/redis-client) to provide fast, simple and elegant solutions to real world scenarios.

#### All Redis Blog application pages 
  * Designing a NoSQL Database using Redis
  * [Painless data migrations using Redis and other schema-less NoSQL datastores](MigrationsUsingSchemalessNoSql)

# Designing a NoSQL Database using Redis 

Oren Eini from the popular .NET blog http://ayende.com/Blog/, is putting together a 
[series of blog posts](http://ayende.com/Blog/archive/2010/04/20/that-no-sql-thing-the-relational-modeling-anti-pattern-in.aspx) explaining how to go about designing a 
simple blog application using a NoSQL database. Although he's using his RavenDB as a reference implementation, this example applies equally well to Redis and other 
NoSQL variants. Some solutions will vary based on the advanced features of each NoSQL solution but the 'core data models' should remain the same.

I will try to add my own thoughts on and where possible show how you can use the advanced features in Redis the C# Client to provide simple, fast and effective solutions. 
The entire source code for this example is available in its simplest form at: 
[BlogPostExample.cs](https://github.com/ServiceStack/ServiceStack.Redis/blob/master/tests/ServiceStack.Redis.Tests/Examples/BlogPostExample.cs). 
I will be re-factoring this solution in what I consider a 'best practices approach' for large applications where I will shove all Redis access behind a repository 
(so the client doesn't know it's even being used) which I will be maintaining at: 
[BlogPostBestPractice.cs](https://github.com/ServiceStack/ServiceStack.Redis/blob/master/tests/ServiceStack.Redis.Tests/Examples/BestPractice/BlogPostBestPractice.cs)

## Modeling Entities in a NoSQL Database 

If you've spent a lot of time building solutions with an RDBMS back-end it can be hard to know which part of your schema is due to the problem domain and which part is the result of an implementation constraint trying to map your ideal domain model onto a relational tabular structure. 

My approach before designing any system is to map out the ideal domain model we need to build in order before I reach for an IDE or a db gui schema creator. Unfortunately creating POCO types is just so damn quick in VS.NET/C#/R# that I've ditched the pencil and paper a long time ago and jump right into using C# automatic properties like a kind of light-weight DSL ripping out entities quicker than I can draw crows feet :)

If you are like me and prefer to design your domain models from POCO types rather than creating RDBMS tables than you're in good shape in implementing a NoSQL solution as the time when you usually morph your pristine domain models into a tabular structure peppering it with Primary and Foreign keys can now saved and put towards a longer lunch break as most of the times you can skip this step entirely.

The schema-less nature of NoSQL databases means you can pretty much store your domain models as-is. You will still need to identify your distinct entities from your Key Value Objects. A good guide I use to help with this is whether the Model only makes sense in the context its parent and whether it is 'co-owned' or referenced by another entity. This is where we start pulling the domain model apart, basically you just replace the collection of strongly-typed entities to a collection of entity ids. Effectively you can think of this like foreign-keys but at a much higher level as you only pull it apart of the domain model when you want to manage the entities independently of each other, not as dictated by your schema.

Taking the User model in it's most simplest form. A User has many blogs, now as you may want to view a list of blogs outside the context of a User (e.g. view a list of newly created blogs, most popular blogs for a category, etc). It becomes a good candidate to being promoted a first class entity.

	//Before
	public class User
	{
	  public int Id { get; set; }
	  public string Name { get; set; }
	  public List<Blog> Blogs { get; set; }
	}

	//After
	public class User
	{
	  public int Id { get; set; }
	  public string Name { get; set; }
	  public List<int> BlogIds { get; set; }
	}


With only a running redis-server instance running and the C# client, the full source to persist and retrieve a list of users is only:

	var redis = new RedisClient();
	using (var redisUsers = redisClient.GetTypedClient<User>())
	{
		redisUsers.Store(new User { Id = redisUsers.GetNextSequence(), Name = "ayende" });
		redisUsers.Store(new User { Id = redisUsers.GetNextSequence(), Name = "mythz" });

		var allUsers = redisUsers.GetAll();

		//Recursively print the values of the POCO
		Console.WriteLine(allUsers.Dump());
	}
	/*Output
	[
		{
			Id: 1,
			Name: ayende,
			BlogIds: []
		},
		{
			Id: 2,
			Name: mythz,
			BlogIds: []
		}
	]
	 */

_Note: [You can also use the generic T.Dump() Extension method yourself](http://www.servicestack.net/mythz_blog/?p=202)._

Ayende has outlined a few scenarios that the Blog application should support. 

  * Main page: show list of blogs
  * Main page: show list of recent posts
  * Main page: show list of recent comments
  * Main page: show tag cloud for posts
  * Main page: show categories
  * Post page: show post and all comments
  * Post page: add comment to post
  * Tag page: show all posts for tag
  * Categories page: show all posts for category

The full source code for the Redis solution for each of these scenarios is [available here](https://github.com/ServiceStack/ServiceStack.Redis/blob/master/tests/ServiceStack.Redis.Tests/Examples/BlogPostExample.cs). Although I will go through each solution in a little more detail below.

### Main page: show list of blogs
Before we can show a list of blogs, we need to add some first. 
Here I make effective use of the Redis client's unique sequence that is maintained for each type.

In identifying my entities I have a general preference for 'automated ids' which are either 
sequential integer ids or Guids - if the entities are going to be distributed across multiple data stores.

Apart from that persisting an object is just a straight forward process of serializing the object graph into a text-serialization format.
By default, the Redis Client uses [Service Stack's JsonSerializer](http://www.servicestack.net/mythz_blog/?p=344) as it's the fastest and JSON Serializer for .NET.

	//Retrieve strongly-typed Redis clients that let's you natively persist POCO's
	using (var redisUsers = redisClient.GetTypedClient<User>())
	using (var redisBlogs = redisClient.GetTypedClient<Blog>())
	{
		//Create the user, getting a unique User Id from the User sequence.
		var mythz = new User { Id = redisUsers.GetNextSequence(), Name = "Demis Bellot" };

		//create some blogs using unique Ids from the Blog sequence. Also adding references
		var mythzBlogs = new List<Blog>
		{
			new Blog
			{
				Id = redisBlogs.GetNextSequence(),
				UserId = mythz.Id,
				UserName = mythz.Name,
				Tags = new List<string> { "Architecture", ".NET", "Redis" },
			},
			new Blog
			{
				Id = redisBlogs.GetNextSequence(),
				UserId = mythz.Id,
				UserName = mythz.Name,
				Tags = new List<string> { "Music", "Twitter", "Life" },
			},
		};
		//Add the blog references
		mythzBlogs.ForEach(x => mythz.BlogIds.Add(x.Id));

		//Store the user and their blogs
		redisUsers.Store(mythz);
		redisBlogs.StoreAll(mythzBlogs);

		//retrieve all blogs
		var blogs = redisBlogs.GetAll();

		Console.WriteLine(blogs.Dump());
	}
	/*Output
	[
		{
			Id: 1,
			UserId: 1,
			UserName: Demis Bellot,
			Tags: 
			[
				Architecture,
				.NET,
				Redis
			],
			BlogPostIds: []
		},
		{
			Id: 2,
			UserId: 1,
			UserName: Demis Bellot,
			Tags: 
			[
				Music,
				Twitter,
				Life
			],
			BlogPostIds: []
		}
	]
	*/


### Main page: show list of recent posts
### Main page: show list of recent comments

For this scenario we can take advantage of Redis's LTRIM'ing operation to maintain custom rolling lists.
The richness of [Redis list operations](http://code.google.com/p/redis/wiki/RpushCommand) also allow us to prepend or append at either end of the list which we take advantage of in this example.


	//Get strongly-typed clients
	using (var redisBlogPosts = redisClient.GetTypedClient<BlogPost>())
	using (var redisComments = redisClient.GetTypedClient<BlogPostComment>())
	{
		//To keep this example let's pretend this is a new list of blog posts
		var newIncomingBlogPosts = redisBlogPosts.GetAll();

		//Let's get back an IList<BlogPost> wrapper around a Redis server-side List.
		var recentPosts = redisBlogPosts.Lists["urn:BlogPost:RecentPosts"];
		var recentComments = redisComments.Lists["urn:BlogPostComment:RecentComments"];

		foreach (var newBlogPost in newIncomingBlogPosts)
		{
			//Prepend the new blog posts to the start of the 'RecentPosts' list
			recentPosts.Prepend(newBlogPost);

			//Prepend all the new blog post comments to the start of the 'RecentComments' list
			newBlogPost.Comments.ForEach(recentComments.Prepend);
		}

		//Make this a Rolling list by only keep the latest 3 posts and comments
		recentPosts.Trim(0, 2);
		recentComments.Trim(0, 2);

		//Print out the last 3 posts:
		Console.WriteLine(recentPosts.GetAll().Dump());
		/* Output: 
		[
			{
				Id: 2,
				BlogId: 2,
				Title: Redis,
				Categories: 
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
						CreatedDate: 2010-04-20T22:14:02.755878Z
					}
				]
			},
			{
				Id: 1,
				BlogId: 1,
				Title: RavenDB,
				Categories: 
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
						CreatedDate: 2010-04-20T22:14:02.755878Z
					},
					{
						Content: Second Comment!,
						CreatedDate: 2010-04-20T22:14:02.755878Z
					}
				]
			},
			{
				Id: 4,
				BlogId: 2,
				Title: Couch Db,
				Categories: 
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
						CreatedDate: 2010-04-20T22:14:02.755878Z
					}
				]
			}
		]
		*/

		Console.WriteLine(recentComments.GetAll().Dump());
		/* Output:
		[
			{
				Content: First Comment!,
				CreatedDate: 2010-04-20T20:32:42.2970956Z
			},
			{
				Content: First Comment!,
				CreatedDate: 2010-04-20T20:32:42.2970956Z
			},
			{
				Content: First Comment!,
				CreatedDate: 2010-04-20T20:32:42.2970956Z
			}
		]
		 */
	}


### Main page: show tag cloud for posts

Redis Sorted Sets provide the perfect data structure to maintain a Tag cloud of all tags.
It's very fast, elegant structure which provides custom-specific operations to maintain and sort the data.


	//Get strongly-typed clients
	using (var redisBlogPosts = redisClient.GetTypedClient<BlogPost>())
	{
		var newIncomingBlogPosts = redisBlogPosts.GetAll();

		foreach (var newBlogPost in newIncomingBlogPosts)
		{
			//For every tag in each new blog post, increment the number of times each Tag has occurred 
			newBlogPost.Tags.ForEach(x =>
				redisClient.IncrementItemInSortedSet("urn:TagCloud", x, 1));
		}

		//Show top 5 most popular tags with their scores
		var tagCloud = redisClient.GetRangeWithScoresFromSortedSetDesc("urn:TagCloud", 0, 4);
		Console.WriteLine(tagCloud.Dump());
	}
	/* Output:
	[
		[
			NoSQL,
			 4
		],
		[
			Scalability,
			 2
		],
		[
			JSON,
			 2
		],
		[
			Redis,
			 1
		],
		[
			Raven,
			 1
		],
	]
	*/


### Main page: show categories

To keep a unique list of categories the right structure to use is a Set.
This allows you to freely add a value multiple times and there will never be any 
duplicates as only one of each value is stored.


	using (var redisBlogPosts = redisClient.GetTypedClient<BlogPost>())
	{
		var blogPosts = redisBlogPosts.GetAll();

		foreach (var blogPost in blogPosts)
		{
			blogPost.Categories.ForEach(x =>
				  redisClient.AddToSet("urn:Categories", x));
		}

		var uniqueCategories = redisClient.GetAllFromSet("urn:Categories");
		Console.WriteLine(uniqueCategories.Dump());
		/* Output:
		[
			DocumentDB,
			NoSQL,
			Cluster,
			Cache
		]
		 */
	}


### Post page: show post and all comments

There is nothing special to do here since comments are Key Value Objects they are 
stored and retrieved with the post, so retrieving the post retrieves it's comments as well.

	var postId = 1;
	using (var redisBlogPosts = redisClient.GetTypedClient<BlogPost>())
	{
		var selectedBlogPost = redisBlogPosts.GetById(postId.ToString());

		Console.WriteLine(selectedBlogPost.Dump());
		/* Output:
		{
			Id: 1,
			BlogId: 1,
			Title: RavenDB,
			Categories: 
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
					CreatedDate: 2010-04-20T21:26:31.9918236Z
				},
				{
					Content: Second Comment!,
					CreatedDate: 2010-04-20T21:26:31.9918236Z
				}
			]
		}
		*/
	}


### Post page: add comment to post

Modifying an entity are one of the strengths of a schema-less data store.
Adding a comment is as simple as 
  * retrieving it's parent post 
  * modifying the POCO entity in memory by adding a comment to the existing list
  * then saving the entity.


	var postId = 1;
	using (var redisBlogPosts = redisClient.GetTypedClient<BlogPost>())
	{
		var blogPost = redisBlogPosts.GetById(postId.ToString());
		blogPost.Comments.Add(
			new BlogPostComment { Content = "Third Post!", CreatedDate = DateTime.UtcNow });
		redisBlogPosts.Store(blogPost);

		var refreshBlogPost = redisBlogPosts.GetById(postId.ToString());
		Console.WriteLine(refreshBlogPost.Dump());
		/* Output:
		{
			Id: 1,
			BlogId: 1,
			Title: RavenDB,
			Categories: 
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
					CreatedDate: 2010-04-20T21:32:39.9688707Z
				},
				{
					Content: Second Comment!,
					CreatedDate: 2010-04-20T21:32:39.9688707Z
				},
				{
					Content: Third Post!,
					CreatedDate: 2010-04-20T21:32:40.2688879Z
				}
			]
		}
		*/
	}


### Tag page: show all posts for tag

Basically in order to view all the posts for a particular category we'll need to provide
a reverse-index by adding all matching post ids into a 'Category > Post Id' Set.

From there it's just a matter of performing a batch request fetching all the Posts with the supplied Ids:


	using (var redisBlogPosts = redisClient.GetTypedClient<BlogPost>())
	{
		var newIncomingBlogPosts = redisBlogPosts.GetAll();

		foreach (var newBlogPost in newIncomingBlogPosts)
		{
			//For each post add it's Id into each of it's 'Cateogry > Posts' index
			newBlogPost.Categories.ForEach(x =>
				  redisClient.AddToSet("urn:Category:" + x, newBlogPost.Id.ToString()));
		}

		//Retrieve all the post ids for the category you want to view
		var documentDbPostIds = redisClient.GetAllFromSet("urn:Category:DocumentDB");

		//Make a batch call to retrieve all the posts containing the matching ids 
		//(i.e. the DocumentDB Category posts)
		var documentDbPosts = redisBlogPosts.GetByIds(documentDbPostIds);

		Console.WriteLine(documentDbPosts.Dump());
		/* Output:
		[
			{
				Id: 4,
				BlogId: 2,
				Title: Couch Db,
				Categories: 
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
						CreatedDate: 2010-04-20T21:38:24.6305842Z
					}
				]
			},
			{
				Id: 1,
				BlogId: 1,
				Title: RavenDB,
				Categories: 
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
						CreatedDate: 2010-04-20T21:38:24.6295842Z
					},
					{
						Content: Second Comment!,
						CreatedDate: 2010-04-20T21:38:24.6295842Z
					}
				]
			}
		]
		 */
	}



_This document is a work in progres..._
