This page shows how to use [ServiceStack's C# Redis Client](~/redis-client/redis-client) to take advantage of the Publish/Subscribe messaging paradigm built into 
[Redis](http://code.google.com/p/redis/) to be able to develop high-performance network notification servers.

# Publish/Subscribe messaging pattern in Redis 
Redis is largely recognized as the most efficient NoSQL solution to manage rich data structures, providing the data source to power stateless back-end app servers. What a lot of people don't know is that it also has a number of characteristics that also make it a prime candidate to develop high-performance networking solutions with. Probably its most noteworthy feature in this area to date, is its built-in Publishing / Subscribe / Messaging support which enables a new range of elegant comet-based and high performance networking solutions to be developed.

Under the covers this is achieved with the [PUBLISH/SUBSCRIBE redis server operations](http://code.google.com/p/redis/wiki/PublishSubscribe). 
Essentially these operations allows any number of clients to be able to *Listen* on any arbitrary named channel. As soon as an external client *Publishes* a message to that channel, 
each *listening* client is notified. The clients will continue to receive messages as long as they maintain at least one *active subscription*.

Service Stack's C# Client exposes this functionality in a similar way as the [redis-rb Ruby client](http://github.com/ezmobius/redis-rb/blob/master/examples/pubsub.rb). 
Essentially you create a *Subscription*, Then *Register* your handlers on each of the events you're interested in, then it's just a matter of *Subscribing* to the channels you're interested in. 
When you want your consumers to stop receiving messages you need to *unsubscribe* from all channels. 

# Pub/Sub Examples 

Below are some examples showing how to use the API to accomplish some basic tasks. At the end of each example is the Console output showing the sequence of events helping you visualize the order 
of each operation. The full runnable source code for these examples are 
[available here](https://github.com/ServiceStack/ServiceStack.Redis/blob/master/tests/ServiceStack.Redis.Tests/Examples/SimplePubSub.cs).

## Example 1 - Publish and receive 5 messages 

	var messagesReceived = 0;

	using (var redisConsumer = new RedisClient(TestConfig.SingleHost))
	using (var subscription = redisConsumer.CreateSubscription())
	{
		subscription.OnSubscribe = channel =>
		{
			Console.WriteLine("Subscribed to '{0}'", channel);
		};
		subscription.OnUnSubscribe = channel =>
		{
			Console.WriteLine("UnSubscribed from '{0}'", channel);
		};
		subscription.OnMessage = (channel, msg) =>
		{
			Console.WriteLine("Received '{0}' from channel '{1}'", msg, channel);

			//As soon as we've received all 5 messages, disconnect by unsubscribing to all channels
			if (++messagesReceived == PublishMessageCount)
			{
				subscription.UnSubscribeFromAllChannels();
			}
		};

		ThreadPool.QueueUserWorkItem(x =>
		{
			Thread.Sleep(200);
			Console.WriteLine("Begin publishing messages...");

			using (var redisPublisher = new RedisClient(TestConfig.SingleHost))
			{
				for (var i = 1; i <= PublishMessageCount; i++)
				{
					var message = MessagePrefix + i;
					Console.WriteLine("Publishing '{0}' to '{1}'", message, ChannelName);
					redisPublisher.PublishMessage(ChannelName, message);
				}
			}
		});

		Console.WriteLine("Started Listening On '{0}'", ChannelName);
		subscription.SubscribeToChannels(ChannelName); //blocking
	}

	Console.WriteLine("EOF");

	/*Output: 
	Started Listening On 'CHANNEL'
	Subscribed to 'CHANNEL'
	Begin publishing messages...
	Publishing 'MESSAGE 1' to 'CHANNEL'
	Received 'MESSAGE 1' from channel 'CHANNEL'
	Publishing 'MESSAGE 2' to 'CHANNEL'
	Received 'MESSAGE 2' from channel 'CHANNEL'
	Publishing 'MESSAGE 3' to 'CHANNEL'
	Received 'MESSAGE 3' from channel 'CHANNEL'
	Publishing 'MESSAGE 4' to 'CHANNEL'
	Received 'MESSAGE 4' from channel 'CHANNEL'
	Publishing 'MESSAGE 5' to 'CHANNEL'
	Received 'MESSAGE 5' from channel 'CHANNEL'
	UnSubscribed from 'CHANNEL'
	EOF
	 */


## Example 2 - Publish 5 messages to 3 different clients 


	const int noOfClients = 3;

	for (var i = 1; i <= noOfClients; i++)
	{
		var clientNo = i;
		ThreadPool.QueueUserWorkItem(x =>
		{
			using (var redisConsumer = new RedisClient(TestConfig.SingleHost))
			using (var subscription = redisConsumer.CreateSubscription())
			{
				var messagesReceived = 0;
				subscription.OnSubscribe = channel =>
				{
					Console.WriteLine("Client #{0} Subscribed to '{1}'", clientNo, channel);
				};
				subscription.OnUnSubscribe = channel =>
				{
					Console.WriteLine("Client #{0} UnSubscribed from '{1}'", clientNo, channel);
				};
				subscription.OnMessage = (channel, msg) =>
				{
					Console.WriteLine("Client #{0} Received '{1}' from channel '{2}'", 
						clientNo, msg, channel);

					if (++messagesReceived == PublishMessageCount)
					{
						subscription.UnSubscribeFromAllChannels();
					}
				};

				Console.WriteLine("Client #{0} started Listening On '{1}'", clientNo, ChannelName);
				subscription.SubscribeToChannels(ChannelName); //blocking
			}

			Console.WriteLine("Client #{0} EOF", clientNo);
		});
	}

	using (var redisClient = new RedisClient(TestConfig.SingleHost))
	{
		Thread.Sleep(500);
		Console.WriteLine("Begin publishing messages...");

		for (var i = 1; i <= PublishMessageCount; i++)
		{
			var message = MessagePrefix + i;
			Console.WriteLine("Publishing '{0}' to '{1}'", message, ChannelName);
			redisClient.PublishMessage(ChannelName, message);
		}
	}

	Thread.Sleep(500);

	/*Output:
	Client #1 started Listening On 'CHANNEL'
	Client #2 started Listening On 'CHANNEL'
	Client #1 Subscribed to 'CHANNEL'
	Client #2 Subscribed to 'CHANNEL'
	Client #3 started Listening On 'CHANNEL'
	Client #3 Subscribed to 'CHANNEL'
	Begin publishing messages...
	Publishing 'MESSAGE 1' to 'CHANNEL'
	Client #1 Received 'MESSAGE 1' from channel 'CHANNEL'
	Client #2 Received 'MESSAGE 1' from channel 'CHANNEL'
	Publishing 'MESSAGE 2' to 'CHANNEL'
	Client #1 Received 'MESSAGE 2' from channel 'CHANNEL'
	Client #2 Received 'MESSAGE 2' from channel 'CHANNEL'
	Publishing 'MESSAGE 3' to 'CHANNEL'
	Client #3 Received 'MESSAGE 1' from channel 'CHANNEL'
	Client #3 Received 'MESSAGE 2' from channel 'CHANNEL'
	Client #3 Received 'MESSAGE 3' from channel 'CHANNEL'
	Client #1 Received 'MESSAGE 3' from channel 'CHANNEL'
	Client #2 Received 'MESSAGE 3' from channel 'CHANNEL'
	Publishing 'MESSAGE 4' to 'CHANNEL'
	Client #1 Received 'MESSAGE 4' from channel 'CHANNEL'
	Client #3 Received 'MESSAGE 4' from channel 'CHANNEL'
	Publishing 'MESSAGE 5' to 'CHANNEL'
	Client #1 Received 'MESSAGE 5' from channel 'CHANNEL'
	Client #3 Received 'MESSAGE 5' from channel 'CHANNEL'
	Client #1 UnSubscribed from 'CHANNEL'
	Client #1 EOF
	Client #3 UnSubscribed from 'CHANNEL'
	Client #3 EOF
	Client #2 Received 'MESSAGE 4' from channel 'CHANNEL'
	Client #2 Received 'MESSAGE 5' from channel 'CHANNEL'
	Client #2 UnSubscribed from 'CHANNEL'
	Client #2 EOF
	 */



# Pub/Sub API 


	public interface IRedisSubscription 
		: IDisposable
	{
		// The number of active subscriptions this client has
		int SubscriptionCount { get; }

		// Registered handler called after client *Subscribes* to each new channel
		Action<string> OnSubscribe { get; set; }

		// Registered handler called when each message is received
		Action<string, string> OnMessage { get; set; }

		// Registered handler called when each channel is unsubscribed
		Action<string> OnUnSubscribe { get; set; }

		// Subscribe to channels by name
		void SubscribeToChannels(params string[] channels);

		// Subscribe to channels matching the supplied patterns
		void SubscribeToChannelsMatching(params string[] patterns);

		void UnSubscribeFromAllChannels();
		void UnSubscribeFromChannels(params string[] channels);
		void UnSubscribeFromChannelsMatching(params string[] patterns);
	}


## Find out more 

For more information on the capabilities of the Publish/Subscribe API check out the 
[PubSub unit tests](https://github.com/ServiceStack/ServiceStack.Redis/blob/master/tests/ServiceStack.Redis.Tests/RedisPubSubTests.cs).
