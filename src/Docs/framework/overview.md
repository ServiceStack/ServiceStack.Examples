[Join the new google group](http://groups.google.com/group/servicestack) or
follow [@demisbellot](http://twitter.com/demisbellot) and [@ServiceStack](http://twitter.com/servicestack)
for twitter updates.  

Service Stack is a high-performance .NET web services framework _(including a number of high-performance sub-components: see below)_ 
that simplifies the development of XML, JSON, JSV and WCF SOAP [Web Services](~/framework/history-of-webservices). 
For more info check out [servicestack.net](http://www.servicestack.net).

Simple REST service example
===========================
    
    //Web Service Host Configuration
    public class AppHost : AppHostBase
    {
        public AppHost() : base("Backbone.js TODO", typeof(TodoService).Assembly) {}

        public override void Configure(Funq.Container container)
        {
            //Register Web Service dependencies
            container.Register(new TodoRepository());

            //Register user-defined RESTful routes			
            Routes
              .Add<Todo>("/todos")
              .Add<Todo>("/todos/{Id}");
        }
    }
    

    //REST Resource DTO
    public class Todo 
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
            if (request.Id != default(long))
                return Repository.GetById(request.Id);

            return Repository.GetAll();
        }

        //Called for both new and updated TODOs
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


### Calling the above TODO REST service from any C#/.NET Client
    //no code-gen required, can re-use above DTO's

    var restClient = new JsonServiceClient("http://localhost/Backbone.Todo");
    var all = restClient.Get<List<Todo>>("/todos"); // Count = 0

    var todo = restClient.Post<Todo>("/todos", new Todo { Content = "New TODO", Order = 1 }); //todo.Id = 1
    all = restClient.Get<List<Todo>>("/todos"); // Count = 1

    todo.Content = "Updated TODO";
    todo = restClient.Post<Todo>("/todos", todo); // todo.Content = Updated TODO
    
    restClient.Delete<Todo>("/todos/" + todo.Id);
    all = restClient.Get<List<Todo>>("/todos"); // Count = 0


### Calling the TODO REST service from jQuery

    $.getJSON("http://localhost/Backbone.Todo/todos", function(todos) {
        alert(todos.length == 1);
    });


That's all the application code required to create a simple REST web service.

##Live Demo of Backbone TODO app (running on Linux/MONO):

**[http://www.servicestack.net/Backbone.Todos/](http://www.servicestack.net/Backbone.Todos/)**

Preview links using just the above code sample with (live demo running on Linux):

ServiceStack's strong-typed nature allows it to infer a greater intelligence of your web services and is able to provide a 
host of functionality for free, out of the box without any configuration required:

  * Host on different formats and endpoints: [XML](http://www.servicestack.net/Backbone.Todos/todos?format=xml), 
    [JSON](http://www.servicestack.net/Backbone.Todos/todos?format=json), [JSV](http://www.servicestack.net/Backbone.Todos/todos?format=jsv),
    [CSV](http://www.servicestack.net/Backbone.Todos/todos?format=csv) 
    
  * [A HTML5 Report format to view your web services data in a human-friendly view](http://www.servicestack.net/Backbone.Todos/todos?format=html)
  
  * [An auto generated api metadata page, with links to your web service XSD's and WSDL's](http://www.servicestack.net/Backbone.Todos/metadata)
  


Download
========

To start developing web services with Service Stack we recommend starting with the ServiceStack.Examples project (includes ServiceStack.dlls):

  * **[ServiceStack.Examples/downloads/](https://github.com/ServiceStack/ServiceStack.Examples/downloads)**

If you already have ServiceStack and just want to download the latest release binaries get them at:

  * **[ServiceStack/downloads/](https://github.com/ServiceStack/ServiceStack/downloads)**

Alternatively if you want keep up with the latest version you can always use the power of Git :)

    git clone git://github.com/ServiceStack/ServiceStack.git

[Release notes for major releases](~/framework/release-notes)

## Getting Started
Online tutorials that walks you through developing and calling web services is available here:

 * **[Read the documentation on the ServiceStack Wiki](~/framework/home)**
 * [Creating a Hello World Web service](http://www.servicestack.net/ServiceStack.Hello/)
 * [Calling Web Services from MonoTouch](http://www.servicestack.net/monotouch/remote-info/)


# Features of a modern web services framework

 Developed in the modern age, Service Stack provides an alternate, cleaner POCO-driven way of creating web services, featuring:

### A DRY, strongly-typed 'pure model' REST Web Services Framework
Unlike other web services frameworks ServiceStack let's you develop web services using strongly-typed models and DTO's.
This lets ServiceStack and other tools to have a greater intelligence about your services allowing:

- [Multiple serialization formats (JSON, XML, JSV and SOAP with extensible plugin model for more)](http://servicestack.net/ServiceStack.Hello/servicestack/metadata)
- [A single re-usable C# Generic Client (In JSON, JSV, XML and SOAP flavors) that can talk to all your services.](https://github.com/ServiceStack/ServiceStack.Extras/blob/master/doc/UsageExamples/UsingServiceClients.cs)
- [Re-use your Web Service DTOs (i.e. no code-gen) on your client applications so you're never out-of-sync](https://github.com/ServiceStack/ServiceStack.Extras/blob/master/doc/UsageExamples/UsingServiceClients.cs)
- [Automatic serialization of Exceptions in your DTOs ResponseStatus](https://github.com/ServiceStack/ServiceStack/blob/master/src/ServiceStack.ServiceInterface/ServiceBase.cs#L154)
- [The possibility of a base class for all your services to put high-level application logic (i.e security, logging, etc)](https://github.com/ServiceStack/ServiceStack/blob/master/src/ServiceStack.ServiceInterface/ServiceBase.cs#L24)
- [Highly testable, your in-memory unit tests for your service can also be used as integration tests](https://github.com/ServiceStack/ServiceStack/blob/master/tests/ServiceStack.WebHost.IntegrationTests/Tests/WebServicesTests.cs)
- [Built-in rolling web service error logging (if Redis is Configured in your AppHost)](https://github.com/ServiceStack/ServiceStack/blob/master/src/ServiceStack.ServiceInterface/ServiceBase.cs#L122)
- [Rich REST and HTML support on all web services with x-www-form-urlencoded & multipart/form-data (i.e. FORM posts and file uploads)](http://servicestack.net/ServiceStack.Hello/)

## Define web services following Martin Fowlers Data Transfer Object Pattern:

Service Stack was heavily influenced by [**Martin Fowlers Data Transfer Object Pattern**](http://martinfowler.com/eaaCatalog/dataTransferObject.html):

>When you're working with a remote interface, such as Remote Facade (388), each call to it is expensive. 
>As a result you need to reduce the number of calls, and that means that you need to transfer more data 
>with each call. One way to do this is to use lots of parameters. 
>However, this is often awkward to program - indeed, it's often impossible with languages such as Java 
>that return only a single value.
>
>The solution is to create a Data Transfer Object that can hold all the data for the call. It needs to be serializable to go across the connection. 
>Usually an assembler is used on the server side to transfer data between the DTO and any domain objects.

The Request and Response DTO's used to define web services in ServiceStack are standard `DataContract` POCO's while the implementation just needs to inherit from a testable and dependency-free `IService<TRequestDto>`. As a bonus for keeping your DTO's in a separate dependency-free .dll, you're able to re-use them in your C#/.NET clients providing a strongly-typed API without any code-gen what-so-ever. Also your DTO's *define everything* Service Stack does not pollute your web services with any additional custom artifacts or markup.

Service Stack re-uses the custom artifacts above and with zero-config and without imposing any extra burden on the developer adds discover-ability and provides hosting of your web service on a number of different physical end-points which as of today includes: XML (+REST), JSON (+REST), JSV (+REST) and SOAP 1.1 / SOAP 1.2.

### WCF the anti-DTO Web Services Framework
Unfortunately this best-practices convention is effectively discouraged by Microsoft's WCF SOAP Web Services framework as they encourage you to develop API-specific RPC method calls by mandating the use method signatures to define your web services API. This results in less re-usable, more client-specific APIs that encourages more remote method calls. 

Unhappy with this perceived anit-pattern in WCF, ServiceStack was born providing a Web Service framework that embraces best-practices for calling remote services, using config-free, convention-based DTO's.


### Full support for unit and integration tests
Your application logic should not be tied to a third party vendor's web service host platform.
In Service Stack they're not, your web service implementations are host and end-point ignorant, dependency-free and can be unit-tested independently of ASP.NET, Service Stack or its IOC.

Without any code changes unit tests written can be re-used and run as integration tests simply by switching the IServiceClient used to point to a configured end-point host.

### Built-in Funq IOC container
Configured to auto-wire all of your web services with your registered dependencies.
[Funq](http://funq.codeplex.com) was chosen for it's [high-performance](http://www.codeproject.com/Articles/43296/Introduction-to-Munq-IOC-Container-for-ASP-NET.aspx), low footprint and intuitive full-featured minimalistic API.

### Encourages development of message-style, re-usable and batch-full web services
Entire POCO types are used to define the request and response DTO's to promote the creation well-defined coarse-grained web services. Message-based interfaces are best-practices when dealing with out-of-process calls as they are can batch more work using less network calls and are ultimately more re-usable as the same operation can be called using different calling semantics. This is in stark contrast to WCF's Operation or Service contracts which encourage RPC-style, application-specific web services by using method signatures to define each operation.

As it stands in general-purpose computing today, there is nothing more expensive you can do than a remote network call. Although easier for the newbie developer, by using _methods_ to define web service operations, WCF is promoting bad-practices by encouraging them to design and treat web-service calls like normal function calls even though they are millions of times slower. Especially at the app-server tier, nothing hurts performance and scalability of your client and server than multiple dependent and synchronous web service calls.

Batch-full, message-based web services are ideally suited in development of SOA services as they result in fewer, richer and more re-usable web services that need to be maintained. RPC-style services normally manifest themselves from a *client perspective* that is the result of the requirements of a single applications data access scenario. Single applications come and go over time while your data and services are poised to hang around for the longer term. Ideally you want to think about the definition of your web service from a *services and data perspective* and how you can expose your data so it is more re-usable by a number of your clients.

### Cross Platform Web Services Framework
With Mono on Linux now reaching full-maturity, Service Stack runs on .NET or Linux with Mono and can either be hosted inside an ASP.NET Web Application, Windows service or Console application running in or independently of a Web Server.

### Low Coupling for maximum accessibility and testability
No coupling between the transport's endpoint and your web service's payload. You can re-use your existing strongly-typed web service DTO's with any .NET client using the available Soap, Xml and Json Service Clients - giving you a strongly-typed API while at the same time avoiding the need for any generated code.

  * The most popular web service endpoints are configured by default. With no extra effort, each new web service created is immediately available and [discoverable](http://www.servicestack.net/ServiceStack.Examples.Host.Web/ServiceStack/Metadata) on the following end points:
    * [XML (+REST)](http://www.servicestack.net/ServiceStack.Examples.Host.Web/ServiceStack/Xml/Metadata?op=GetFactorial)
    * [JSON (+REST)](http://www.servicestack.net/ServiceStack.Examples.Host.Web/ServiceStack/Json/Metadata?op=GetFactorial)
    * [JSV (+REST)](http://www.servicestack.net/ServiceStack.Examples.Host.Web/ServiceStack/Jsv/Metadata?op=GetFactorial)
    * [SOAP 1.1](http://www.servicestack.net/ServiceStack.Hello/servicestack/soap11)
    * [SOAP 1.2](http://www.servicestack.net/ServiceStack.Hello/servicestack/soap12)
  * View the [Service Stack endpoints page](~/framework/history-of-webservices) for our recommendations on which endpoint to use and when.

# High Performance Sub Projects
Also included in ServiceStack are libraries that are useful in the development of high performance web services:

 * [ServiceStack.Text](~/text-serializers/json-csv-jsv-serializers) - The home of ServiceStack's JSON and JSV text serializers, the fastest text serializers for .NET
   * [JsonSerializer](http://www.servicestack.net/mythz_blog/?p=344) - The fastest JSON Serializer for .NET. Over 3 times faster than other .NET JSON serialisers.
   * [TypeSerializer](~/text-serializers/json-csv-jsv-serializers) - The JSV-format, a fast, compact text serializer that is very resilient to schema changes and is:
       * 3.5x quicker and 2.6x smaller than the .NET XML DataContractSerializer and
       * 5.3x quicker and 1.3x smaller than the .NET JSON DataContractSerializer - _[view the detailed benchmarks](http://www.servicestack.net/benchmarks/NorthwindDatabaseRowsSerialization.1000000-times.2010-02-06.html)_

 * [ServiceStack.Redis](~/redis-client/redis-client) - An API complete C# [Redis](http://code.google.com/p/redis/) client with native support for persisting C# POCO objects.
   *  You can download the latest [RedisWindowsDownload Windows build for the Redis Server here].
   * [Redis Admin UI](http://www.servicestack.net/mythz_blog/?p=381) - An Ajax GUI admin tool to help visualize your Redis data.

 * [OrmLite](~/ormlite/ormlite-overview) - A convention-based, configuration free lightweight ORM that uses attributes from DataAnnotations to infer the table schema. Currently supports both Sqlite and SqlServer.

 * [Caching](~/framework/caching-options) - A common interface for caching with providers for:
   * Memcached
   * In Memory Cache
   * Redis


## Find out More
 * Twitter: to get updates of ServiceStack, follow [@demisbellot](http://twitter.com/demisbellot) or [@ServiceStack](http://twitter.com/ServiceStack) on twitter.
 * Email any questions to _demis.bellot@gmail.com_

## Future Roadmap
Service Stack is under continuous improvement and is always adding features that are useful for high-performance, scalable and resilient web service scenarios. This is the current road map but is open to change.
If you have suggestions for new features or want to prioritize the existing ones below: [http://code.google.com/p/servicestack/issues/entry you can leave feedback here].

 * Add an opt-in durable Message Queue service processing of [AsyncOneWay] requests (with In Memory, Redis and RabbitMQ implementations)
 * Enable [ProtoBuf.NET](http://code.google.com/p/protobuf-net/), TypeSerializer and CSV (for tabular datasets) web service endpoints
 * Integrate the Spark View Engine and enable a HTML endpoint so web services can also return a HTML human-friendly view
 * New REST web service endpoint developed in the 'Spirit of REST' with partitioning of GET / POST / PUT / DELETE requests and utilizing 'Accept mimetype' HTTP request header to determine the resulting content type.
 * Code generated proxy classes for Objective-C, Java Script / Action Script (and Java or C++ if there's enough interest) clients.
 * Develop a completely managed HTTP Web Service Host (at the moment looking at building on top of [Kayak HTTP](http://kayakhttp.com))
 * Add support for 'Web Sockets' protocol

## Similar open source projects
Similar Open source .NET projects for developing or accessing web services include:

 * Open Rasta - RESTful web service framework:
     * [http://trac.caffeine-it.com/openrasta](http://trac.caffeine-it.com/openrasta)

 * Rest Sharp - an open source REST client for .NET
     * [http://restsharp.org](http://restsharp.org)


## Troubleshooting

For IIS 6.0-only web servers (i.e. without IIS 7 and compatibility-mode) IIS and ASP.NET requires mapping an extension in order to embed ServiceStack. You can use a default handled ASP.NET extension like *.ashx e.g.

    <add path="servicestack.ashx" type="ServiceStack.WebHost.Endpoints.ServiceStackHttpHandlerFactory, ServiceStack" verb="*"/>

Which will change your urls will now look like:

    http://localhost/servicestack.ashx/xml/syncreply/Hello?Name=World



***

Runs on both Mono and .NET 3.5. _(Live preview hosted on Mono / CentOS)_

