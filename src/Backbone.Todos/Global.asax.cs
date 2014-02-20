using System;
using ServiceStack;
using ServiceStack.Redis;
using Funq;
using ServiceStack.Text;

//The entire C# source code for the ServiceStack + Redis TODO REST backend. There is no other .cs :)
namespace Backbone.Todos
{
    // Define your ServiceStack web service request (i.e. Request DTO).
    public class Todo
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public int Order { get; set; }
        public bool Done { get; set; }
    }

    // Create your ServiceStack rest-ful web service implementation. 
    public class TodoService : Service
    {
        public object Get(Todo todo)
        {
            //Return a single Todo if the id is provided.
            if (todo.Id != default(long))
                return Redis.As<Todo>().GetById(todo.Id);

            //Return all Todos items.
            return Redis.As<Todo>().GetAll();
        }

        // Handles creating and updating the Todo items.
        public Todo Post(Todo todo)
        {
            var redis = Redis.As<Todo>();
            
            //Get next id for new todo
            if (todo.Id == default(long)) 
                todo.Id = redis.GetNextSequence();
            
            redis.Store(todo);
            
            return todo;
        }

        // Handles creating and updating the Todo items.
        public Todo Put(Todo todo)
        {
            return Post(todo);
        }

        // Handles Deleting the Todo item
        public void Delete(Todo todo)
        {
            Redis.As<Todo>().DeleteById(todo.Id);
        }
    }

    // Create your ServiceStack web service application with a singleton AppHost.
    public class AppHost : AppHostBase
    {
        // Initializes a new instance of your ServiceStack application, with the specified name and assembly containing the services.
        public AppHost() : base("Backbone.js TODO", typeof(TodoService).Assembly) { }

        // Configure the container with the necessary routes for your ServiceStack application.
        public override void Configure(Container container)
        {
            //Configure ServiceStack Json web services to return idiomatic Json camelCase properties.
            JsConfig.EmitCamelCaseNames = true;

            //Register Redis factory in Funq IoC. The default port for Redis is 6379.
            container.Register<IRedisClientsManager>(new BasicRedisClientManager("localhost:6379"));

            //Register user-defined REST Paths using the fluent configuration API
            Routes
              .Add<Todo>("/todos")
              .Add<Todo>("/todos/{Id}");
        }
    }

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //Initialize your ServiceStack AppHost
            (new AppHost()).Init();
        }
    }
}
