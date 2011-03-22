using System;
using Funq;
using ServiceStack.Redis;
using ServiceStack.ServiceInterface;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.ServiceHost;

namespace Backbone.Todos
{
	[RestService("/todos")]
	[RestService("/todos/{Id}")]
	public class Todo
	{
		public long Id { get; set; }

		public string Content { get; set; }

		public int Order { get; set; }

		public bool Done { get; set; }
	}

	public class TodoService : RestServiceBase<Todo>
	{
		public IRedisClientsManager RedisManager { get; set; }

		public override object OnGet(Todo request)
		{
			if (request.Id == default(long)) 
				return RedisManager.ExecAs<Todo>(r => r.GetAll());

			return  RedisManager.ExecAs<Todo>(r => r.GetById(request.Id));
		}

		public override object OnPost(Todo todo)
		{
			RedisManager.ExecAs<Todo>(r => {
				if (todo.Id == default(long)) todo.Id = r.GetNextSequence();
				r.Store(todo);
			});

			return todo;
		}

		public override object OnDelete(Todo request)
		{
			RedisManager.ExecAs<Todo>(r => r.DeleteById(request.Id));
			return null;
		}
	}

	public class AppHost : AppHostBase
	{
		public AppHost() : base("Backbone.js TODO", typeof(TodoService).Assembly) { }

		public override void Configure(Container container)
		{
			container.Register<IRedisClientsManager>(new BasicRedisClientManager("localhost:6379"));
		}
	}

	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			new AppHost().Init();
		}
	}
}