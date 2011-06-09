Latest release notes for the latest version of the c# [ServiceStack.Redis](~/redis-client/redis-client).

# Redis Client v1.30 Release Notes
This version adds support for the recently released [Redis v2.0](http://code.google.com/p/redis/wiki/Redis_2_0_0_Changelog). 
This release also fixes a major performance issue with the older clients so it is now a recommended upgrade for all older clients which ideally should coincide with an upgrade to  redis-server v2.0.

## Warning: Breaking API Changes
Unfortunately during development of [Redis Web Services](http://www.servicestack.net/RedisWebServices.Host/Public/Metadata) I became aware that the nomenclature 
of the descriptive API found in the [IRedisClient] and [IRedisTypedClient] were sometimes in-consistent. Normally when making changes to published API's I would 
continue to deprecate and support the older API's unfortunately since there was _so many_ changes supporting both API's would've made it more confusing so I've 
taken the opportunity with the release of redis-server v2.0 to start with a fresh slate, though I will continue to make available the older version (v1.20) of the client below.

### The new terminology used in the new API
The wording in the new [IRedisClient] and [RedisWebService](http://www.servicestack.net/RedisWebServices.Host/Public/Metadata) API's:
  * For write operations I specify to *what* I'm adding an as well as *where* I'm adding it to: 
     * e.g. `AddItemToList` as opposed to `AddToList`.
  * For read operations I specify *what* I'm getting and *where* I'm getting it from 
    * e.g. `GetAllItemsFromList` as opposed to `GetAllFromList`.
  * A normal Key/Value pair using Get/Set/SetEntryInHash is referred to as an `Entry` which contains both a 'Key' and a 'Value'. So the normal `Set` becomes `SetEntry`
  * A Value in a List, Set, or SortedSet is referred as an `Item`

Please file any issues you find with the C# Client (or the rest of ServiceStack) on the [issues pages](https://github.com/ServiceStack/ServiceStack.Redis/issues).

# Download 
The latest version of the open source c# redis client is available:
  * As a stand-alone dll - https://github.com/downloads/mythz/ServiceStack.Redis/ServiceStack.Redis.zip
  * Or bundled inside the core Service Stack binaries - https://github.com/downloads/mythz/ServiceStack/ServiceStack.zip

Download the stable and development release of the Redis Server Windows builds here:
http://code.google.com/p/servicestack/wiki/RedisWindowsDownload (currently v2.0RC)

## Older Releases 
For those that can't move to the new redis-server and new C# client, I will maintain the downloads of the older v2.0 libraries:
  * [ServiceStack.Redis-1.20.zip](http://servicestack.googlecode.com/files/ServiceStack.Redis-1.20.zip)
  * [ServiceStack-1.20.zip](http://servicestack.googlecode.com/files/ServiceStack-1.20.zip)



# Redis Client v1.20 Release Notes 
This version adds support for *all operations* available in Redis v1.3.11 (and the upcoming v2.0 RC) apart from the special debug commands: MONITOR / CONFIG / DEBUG 
which are commonly accessed via redis-cli/telnet for diagnosing a live-running Redis instance.

# Redis Client v1.19 Release Notes 
The biggest feature of this release is support for Redis Publish/Subscribe operations.
Other than that all client client API's now use the new binary safe Redis wire protocol so now all arguments including keys are binary safe strings. 
Most new Redis operations available in the latest version of redis-server v1.3.10 have been implemented.

Other noteworthy mentions:
  * *AcquireLock* with built in backoff-sleep-multiplier was added on both [IRedisClient] and [IRedisTypedClient] to provide an elegant way to achieve application level locks.  
  * The Refactoring process to change all commands to use the new protocol added more tests which discovered and fixed more bugs.
  * Issues identified in some hash commands resulted in more tests and fixes. -- thanks for reporting 

New or changed API methods:
  * *[RedisPubSub Publish/Subscribe/PSubscribe/UnSubscribe/PUnSubscribe]* added to support Redis Pub/Sub
  * *Append* / *Substring* string operations requiring v1.3.10 redis-server
  * *SetString* with TimeOut now uses the more efficient *SetEx* operation
  * *GetAllWithScoresFromSortedSet* 
  * *Sort* has been overhauled to allow for simpler opt-in usage.

= Redis Client v1.14 Release Notes =
This release brings the client up to date with v1.3.9 of the redis-server with the following new operations included:
  * [HINCRBY](http://code.google.com/p/redis/wiki/HincrbyCommand)
  * [HMSET](http://code.google.com/p/redis/wiki/HmsetCommand)
  * [HSETNX](http://code.google.com/p/redis/wiki/HsetnxCommand) (only available in redis trunk)
You can access these new operations with the IRedisClient using the v1.3.9 of the redis-server.

Richer Transaction support has been improved with the addition of the [IRedisTypedTransaction IRedisTypedTransaction<T>] which utilizes the 
[IRedisTypedClient](~/redis-client/iredistypedclient-api) 
to provide transactions against strongly-typed POCO types. Examples can be found on the test page 
[RedisTypedTransactionTests.cs](https://github.com/ServiceStack/ServiceStack.Redis/blob/master/tests/ServiceStack.Redis.Tests/Generic/RedisTypedTransactionTests.cs)

# Redis Client v1.09 Release Notes 

## Features 
The biggest feature in this release is the addition of Redis Transactions. Almost every command in [IRedisClient] can now be queued and executed as part of an atomic Redis operation. For more info on how to use transactions visit [RedisTransactions].

## Other noteworthy features 
 * Fixed some bugs and provided richer support for Redis Hashes including implementing a the .NET native generic `IDictionary<string,string>` interface.
 * Name changes for a couple SortedSet and Hash operations for better consistency and readability.

# Older Release Notes 

## Redis Client v1.08 Release Notes 

### API compatibility 
This version of the client includes support for the *Redis 1.3.7 API*. This includes support for all Sorted Sets and Hash operations.

### Features 
  * Included in the release is preliminary support for Transient Redis Message Queues.
  * Thread-safe load balanced `PooledRedisClientManager` and `BasicRedisClientManager` client managers, suitable to drop in any IOC.
      * Supports configuration of multiple read-write masters and read-only slave Redis server instances.

### Breaking API changes: 
  * Due to a change in the Redis 1.3.7 KEYS operation, `IRedisClient.AllKeys` and `IRedisClient.GetKeys()` now return a `List<string>`.
  