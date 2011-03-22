using System;
using Funq;
using ServiceStack.Redis;
using ServiceStack.ServiceInterface;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.ServiceHost;

namespace Backbone.Todos
{
	//Register REST Paths
	[RestService("/todos")]
	[RestService("/todos/{Id}")]
	public class Todo //REST Resource Model
	{
		public long Id { get; set; }

		public string Content { get; set; }

		public int Order { get; set; }

		public bool Done { get; set; }
	}

	//Implement TODO Rest Service
	public class TodoService : RestServiceBase<Todo>
	{
		//Injected by IOC
		public IRedisClientsManager RedisManager { get; set; }

		public override object OnGet(Todo request)
		{
			//return all todos
			if (request.Id == default(long))
				return RedisManager.ExecAs<Todo>(r => r.GetAll());

			//return single todo
			return RedisManager.ExecAs<Todo>(r => r.GetById(request.Id));
		}

		//Handles creaing a new and updating existing todo
		public override object OnPost(Todo todo)
		{
			RedisManager.ExecAs<Todo>(r => {
				//Get next id for new todo
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

	//Configure ServiceStack.NET web service
	public class AppHost : AppHostBase
	{
		//Tell ServiceStack name and where to find your web services
		public AppHost() : base("Backbone.js TODO", typeof(TodoService).Assembly) { }

		public override void Configure(Container container)
		{
			//Register Redis factory in Funq IOC
			container.Register<IRedisClientsManager>(new BasicRedisClientManager("localhost:6379"));
		}
	}

	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			//Start ServiceStack App
			new AppHost().Init();
		}
	}
}