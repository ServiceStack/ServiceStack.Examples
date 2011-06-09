This page provides examples on how to create atomic Redis transactions with [ServiceStackRedis Service Stack's C# Redis Client]

# How to create custom atomic operations in Redis 

One of the main features of Redis is the ability to construct custom atomic operations. This is achieved by utilizing Redis's 
[MULTI/EXEC/DISCARD](http://code.google.com/p/redis/wiki/MultiExecCommand) operations.

[ServiceStack's C# Redis Client](~/redis-client/redis-client) makes it easy to utilize Redis transactions by providing a strongly-typed [IRedisTransaction](IRedisTransaction)
(for strings) and [IRedisTypedTransaction<T>](IRedisTypedTransaction) (for POCO types) APIs with convenience methods to allow you to combine any [IRedisClient](IRedisClient) operation within a single transaction.

Creating a transaction is done by calling `IRedisClient.CreateTransaction()`. 
From there you 'Queue' up all operations you want to be apart of the transaction by using one of the `IRedisTransaction.QueueCommand()` overloads. After that you can execute all the operations by calling `IRedisTransaction.Commit()` which will send the 'EXEC' command to the Redis server executing all the Queued commands and processing their callbacks.

If you don't call the `Commit()` before the end of the using block, `Dispose()` method will automatically invokes `Rollback()` that will send the 'DISCARD' command disposing of the current transaction and resetting the Redis client connection back to its previous state.

## Redis Transaction Examples 

Below is a simple example showing how to queue up Redis operations with and without a callback.


	int callbackResult;
	using (var trans = redis.CreateTransaction())
	{
	  trans.QueueCommand(r => r.Increment("key"));  
	  trans.QueueCommand(r => r.Increment("key"), i => callbackResult = i);  

	  trans.Commit();
	}
	//The value of "key" is incremented twice. The latest value of which is also stored in 'callbackResult'.


### Other common examples 
The full-source code and other common examples can be found on the 
[common transaction tests page](https://github.com/ServiceStack/ServiceStack.Redis/blob/master/tests/ServiceStack.Redis.Tests/RedisTransactionCommonTests.cs).

	[Test]
	public void Can_Set_and_Expire_key_in_atomic_transaction()
	{
		var oneSec = TimeSpan.FromSeconds(1);

		Assert.That(Redis.GetString("key"), Is.Null);
		using (var trans = Redis.CreateTransaction())                  //Calls 'MULTI'
		{
			trans.QueueCommand(r => r.SetString("key", "a"));      //Queues 'SET key a'
			trans.QueueCommand(r => r.ExpireKeyIn("key", oneSec)); //Queues 'EXPIRE key 1'

			trans.Commit();                                        //Calls 'EXEC'

		}                                                              //Calls 'DISCARD' if 'EXEC' wasn't called

		Assert.That(Redis.GetString("key"), Is.EqualTo("a"));
		Thread.Sleep(TimeSpan.FromSeconds(2));
		Assert.That(Redis.GetString("key"), Is.Null);
	}

	[Test]
	public void Can_Pop_priority_message_from_SortedSet_and_Add_to_workq_in_atomic_transaction()
	{
		var messages = new List<string> { "message4", "message3", "message2" };

		Redis.AddToList("workq", "message1");

		var priority = 1;
		messages.ForEach(x => Redis.AddToSortedSet("prioritymsgs", x, priority++));

		var highestPriorityMessage = Redis.PopFromSortedSetItemWithHighestScore("prioritymsgs");

		using (var trans = Redis.CreateTransaction())
		{
			trans.QueueCommand(r => r.RemoveFromSortedSet("prioritymsgs", highestPriorityMessage));
			trans.QueueCommand(r => r.AddToList("workq", highestPriorityMessage));	

			trans.Commit();											
		}

		Assert.That(Redis.GetAllFromList("workq"), 
			Is.EquivalentTo(new List<string> { "message1", "message2" }));
		Assert.That(Redis.GetAllFromSortedSet("prioritymsgs"), 
			Is.EquivalentTo(new List<string> { "message3", "message4" }));
	}


## All-in-one example 
This and other examples can be found by looking at the 
[RedisTransactionTests.cs test suite](https://github.com/ServiceStack/ServiceStack.Redis/blob/master/tests/ServiceStack.Redis.Tests/RedisTransactionTests.cs).

Here is an all in one examples combining many different Redis operations within a single transaction:

	[Test]
	public void Supports_different_operation_types_in_same_transaction()
	{
		var incrementResults = new List<int>();
		var collectionCounts = new List<int>();
		var containsItem = false;

		Assert.That(Redis.GetString(Key), Is.Null);
		using (var trans = Redis.CreateTransaction())
		{
			trans.QueueCommand(r => r.Increment(Key), intResult => incrementResults.Add(intResult));
			trans.QueueCommand(r => r.AddToList(ListKey, "listitem1"));
			trans.QueueCommand(r => r.AddToList(ListKey, "listitem2"));
			trans.QueueCommand(r => r.AddToSet(SetKey, "setitem"));
			trans.QueueCommand(r => r.SetContainsValue(SetKey, "setitem"), b => containsItem = b);
			trans.QueueCommand(r => r.AddToSortedSet(SortedSetKey, "sortedsetitem1"));
			trans.QueueCommand(r => r.AddToSortedSet(SortedSetKey, "sortedsetitem2"));
			trans.QueueCommand(r => r.AddToSortedSet(SortedSetKey, "sortedsetitem3"));
			trans.QueueCommand(r => r.GetListCount(ListKey), intResult => collectionCounts.Add(intResult));
			trans.QueueCommand(r => r.GetSetCount(SetKey), intResult => collectionCounts.Add(intResult));
			trans.QueueCommand(r => r.GetSortedSetCount(SortedSetKey), intResult => collectionCounts.Add(intResult));
			trans.QueueCommand(r => r.Increment(Key), intResult => incrementResults.Add(intResult));

			trans.Commit();
		}

		Assert.That(containsItem, Is.True);
		Assert.That(Redis.GetString(Key), Is.EqualTo("2"));
		Assert.That(incrementResults, Is.EquivalentTo(new List<int> { 1, 2 }));
		Assert.That(collectionCounts, Is.EquivalentTo(new List<int> { 2, 1, 3 }));
		Assert.That(Redis.GetAllFromList(ListKey), Is.EquivalentTo(new List<string> { "listitem1", "listitem2" }));
		Assert.That(Redis.GetAllFromSet(SetKey), Is.EquivalentTo(new List<string> { "setitem" }));
		Assert.That(Redis.GetAllFromSortedSet(SortedSetKey), Is.EquivalentTo(new List<string> { "sortedsetitem1", "sortedsetitem2", "sortedsetitem3" }));
	}
