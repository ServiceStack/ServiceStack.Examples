The [Redis Transactions](~/redis-client/redis-transactions) interface implemented by [ServiceStack's C# Redis Client](~/redis-client/redis-client).

# Introduction 

Redis transaction interface provides useful overloads that let you Queue-up any [IRedisTypedClient] operation within a single transaction. 
The API provides support for a callback so you also have access to any return values returned as part of the transaction as well.

# Usage 

Below is a simple example showing how to access, use and commit the transaction.

	var typedClient = Redis.GetTypedClient<MyPocoType>();			
	using (var trans = typedClient.CreateTransaction())
	{
		trans.QueueCommand(r => r.Set("nosqldbs", new MyPocoType {Id = 1, Name = "Redis"));

		trans.Commit();
	}


For a transaction that operates on string values see [IRedisTransaction].

# Details 

	public interface IRedisTypedTransaction<T> 
		: IDisposable
	{
		void QueueCommand(Action<IRedisTypedClient<T>> command);
		void QueueCommand(Action<IRedisTypedClient<T>> command, Action onSuccessCallback);
		void QueueCommand(Action<IRedisTypedClient<T>> command, Action onSuccessCallback, Action<Exception> onErrorCallback);

		void QueueCommand(Func<IRedisTypedClient<T>, int> command);
		void QueueCommand(Func<IRedisTypedClient<T>, int> command, Action<int> onSuccessCallback);
		void QueueCommand(Func<IRedisTypedClient<T>, int> command, Action<int> onSuccessCallback, Action<Exception> onErrorCallback);

		void QueueCommand(Func<IRedisTypedClient<T>, bool> command);
		void QueueCommand(Func<IRedisTypedClient<T>, bool> command, Action<bool> onSuccessCallback);
		void QueueCommand(Func<IRedisTypedClient<T>, bool> command, Action<bool> onSuccessCallback, Action<Exception> onErrorCallback);

		void QueueCommand(Func<IRedisTypedClient<T>, double> command);
		void QueueCommand(Func<IRedisTypedClient<T>, double> command, Action<double> onSuccessCallback);
		void QueueCommand(Func<IRedisTypedClient<T>, double> command, Action<double> onSuccessCallback, Action<Exception> onErrorCallback);

		void QueueCommand(Func<IRedisTypedClient<T>, byte[]> command);
		void QueueCommand(Func<IRedisTypedClient<T>, byte[]> command, Action<byte[]> onSuccessCallback);
		void QueueCommand(Func<IRedisTypedClient<T>, byte[]> command, Action<byte[]> onSuccessCallback, Action<Exception> onErrorCallback);

		void QueueCommand(Func<IRedisTypedClient<T>, string> command);
		void QueueCommand(Func<IRedisTypedClient<T>, string> command, Action<string> onSuccessCallback);
		void QueueCommand(Func<IRedisTypedClient<T>, string> command, Action<string> onSuccessCallback, Action<Exception> onErrorCallback);

		void QueueCommand(Func<IRedisTypedClient<T>, T> command);
		void QueueCommand(Func<IRedisTypedClient<T>, T> command, Action<T> onSuccessCallback);
		void QueueCommand(Func<IRedisTypedClient<T>, T> command, Action<T> onSuccessCallback, Action<Exception> onErrorCallback);

		void QueueCommand(Func<IRedisTypedClient<T>, List<string>> command);
		void QueueCommand(Func<IRedisTypedClient<T>, List<string>> command, Action<List<string>> onSuccessCallback);
		void QueueCommand(Func<IRedisTypedClient<T>, List<string>> command, Action<List<string>> onSuccessCallback, Action<Exception> onErrorCallback);

		void QueueCommand(Func<IRedisTypedClient<T>, List<T>> command);
		void QueueCommand(Func<IRedisTypedClient<T>, List<T>> command, Action<List<T>> onSuccessCallback);
		void QueueCommand(Func<IRedisTypedClient<T>, List<T>> command, Action<List<T>> onSuccessCallback, Action<Exception> onErrorCallback);

		void Commit();
		void Rollback();
	}
