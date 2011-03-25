using System.Collections.Generic;
using System.Linq;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

//Entire implementation for the backend REST service of the TODO demo app
namespace StarterTemplates.Common
{
	//Register REST Paths
	[RestService("/todos")]
	[RestService("/todos/{Id}")]
	public class Todo //REST Resource DTO
	{
		public long Id { get; set; }
		public string Content { get; set; }
		public int Order { get; set; }
		public bool Done { get; set; }
	}

	//Todo REST Service implementation
	public class TodoService : RestServiceBase<Todo>
	{
		public TodoRepository Repository { get; set; }  //Injected by IOC

		public override object OnGet(Todo request)
		{
			if (request.Id == default(long))
				return Repository.GetAll();

			return Repository.GetById(request.Id);
		}

		//Called for new and update
		public override object OnPost(Todo todo)
		{
			return Repository.Store(todo);
		}

		public override object OnDelete(Todo request)
		{
			Repository.DeleteById(request.Id);
			return null;
		}
	}


	/// <summary>
	/// In-memory repository, so we can run the TODO app without any dependencies
	/// Registered in Funq as a singleton, injected on every request
	/// </summary>
	public class TodoRepository
	{
		private readonly List<Todo> todos = new List<Todo>();

		public List<Todo> GetAll()
		{
			return todos;
		}

		public Todo GetById(long id)
		{
			return todos.FirstOrDefault(x => x.Id == id);
		}

		public Todo Store(Todo todo)
		{
			if (todo.Id == default(long))
			{
				todo.Id = todos.Count == 0 ? 1 : todos.Max(x => x.Id) + 1;
			}
			else
			{
				for (var i = 0; i < todos.Count; i++)
				{
					if (todos[i].Id != todo.Id) continue;

					todos[i] = todo;
					return todo;
				}
			}

			todos.Add(todo);
			return todo;
		}

		public void DeleteById(long id)
		{
			todos.RemoveAll(x => x.Id == id);
		}
	}
}