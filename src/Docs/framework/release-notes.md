#ServiceStack 3.09 Release Notes

## Latest version of Dapper built-in

Now in ServiceStack is StackOverflow's own [benchmark leading](http://www.servicestack.net/benchmarks/) Micro ORM **[Dapper](http://code.google.com/p/dapper-dot-net/)**.
This means a fresh NuGet install of ServiceStack now includes the 2 fastest Micro ORMS for .NET! :)

OrmLite and Dapper are very similar in design in that they're simply useful extension methods on ADO.NET's `System.Data.*` interfaces, the difference being Dapper has extension methods of `IDbConnection` whilst OrmLite methods hangs off the lower `IDbCommand`. And because they both make use of 'clean POCOs' - they can be used interchangibly together on the same DB connection. This also allows them to both make use of `OrmLiteConnectionFactory` to configure connection manager over your DB ConnectionString.

## Mvc Mini Profiler now baked in

We've made ServiceStack's [HTML5 JSON Report Format](https://github.com/ServiceStack/ServiceStack/wiki/HTML5ReportFormat) even better by now including the excellent [Mvc Mini Profiler](http://code.google.com/p/mvc-mini-profiler/) - by [@jarrod_dixon](https://twitter.com/jarrod_dixon) and [@samsaffron](https://twitter.com/samsaffron).
It's the same profiler used to profile and help speed up sites like [Stack Overflow](http://www.stackoverflow.com) and more recently the much faster [NuGet v2.0](http://nuget.org) website.

As the MVC Mini Profiler is optimized for a .NET 4.0 MVC app, we've made some changes in order to integrate it into ServiceStack:
  
  - Make it work in .NET 3.0 by backporting .NET 4.0 classes into **ServiceStack.Net30** namespace (Special thanks to OSS! :)
    - Using Mono's **ConcurrentDictionary** classes
    - Using [Lokad.com's Tuples](http://code.google.com/p/lokad-shared-libraries/source/browse/Source/Lokad.Shared/Tuples/Tuple.cs)
  - Switched to using ServiceStack's much faster [Json Serializer](http://www.servicestack.net/docs/text-serializers/json-serializer)
  - Reduced the overall footprint by replacing the use of jQuery and jQuery.tmpl with a much smaller [jquip (jQuery-in-parts)](https://github.com/mythz/jquip) dependency.
  - Moved to the **ServiceStack.MiniProfiler** namespace and renamed to **Profiler** to avoid clashing with another Mvc Mini Profiler in the same project

As a side-effect of integrating the Mvc Mini Profiler all ServiceStack .NET 3.0 projects can make use of .NET 4.0's ConcurrentDictionary and Tuple support, hassle free!

### Using the MVC Mini Profiler

Just like the [Normal Mvc Mini Profiler](http://code.google.com/p/mvc-mini-profiler/) you can enable it by starting it in your Global.asax, here's how to enable it for local requests:
 
  protected void Application_BeginRequest(object src, EventArgs e)
  {
    if (Request.IsLocal)
      Profiler.Start();
  }

  protected void Application_EndRequest(object src, EventArgs e)
  {
    Profiler.Stop();
  }

That's it! Now everytime you view a web service in your browser (locally) you'll see a profiler view of your service broken down in different stages:

![Hello MiniProfiler](http://www.servicestack.net/files/miniprofiler-hello.png)

By default you get to see how long it took ServiceStack to de-serialize your request, run any Request / Response Filters and more importantly how long it took to **Execute** your service.

The profiler includes special support for SQL Profiling that can easily be enabled for OrmLite and Dapper by getting it to use a Profiled Connection using a ConnectionFilter:

  this.Container.Register<IDbConnectionFactory>(c =>
    new OrmLiteConnectionFactory(
      "~/App_Data/db.sqlite".MapHostAbsolutePath(),
      SqliteOrmLiteDialectProvider.Instance) {
        ConnectionFilter = x => new ProfiledDbConnection(x, Profiler.Current)
      });

Refer to the [Main MVC MiniProfiler home page](http://code.google.com/p/mvc-mini-profiler/) for instructions on how to configure profiling for Linq2Sql and EntityFramework.

It's also trivial to add custom steps enabling even finer-grained profiling for your services. 
Here's a [simple web service DB example](https://github.com/ServiceStack/ServiceStack/blob/master/tests/ServiceStack.WebHost.IntegrationTests/Services/ProfilerService.cs) 
returning a list of Movies using both a simple DB query and a dreaded N+1 query.

  public class MiniProfiler
  {
    public string Type { get; set; }
  }

  public class MiniProfilerService : ServiceBase<MiniProfiler>
  {
    public IDbConnectionFactory DbFactory { get; set; }

    protected override object Run(MiniProfiler request)
    {
      var profiler = Profiler.Current;

      using (var dbConn = DbFactory.OpenDbConnection())
      using (var dbCmd = dbConn.CreateCommand())
      using (profiler.Step("MiniProfiler Service"))
      {
        if (request.Type == "n1")
        {
          using (profiler.Step("N + 1 query"))
          {
            var results = new List<Movie>();
            foreach (var movie in dbCmd.Select<Movie>())
            {
              results.Add(dbCmd.QueryById<Movie>(movie.Id));
            }
            return results;
          }
        }

        using (profiler.Step("Simple Select all"))
        {
          return dbCmd.Select<Movie>();
        }
      }
    }
  }

Calling the above service normally provides the following Profiler output:

![Simple DB Example](http://www.servicestack.net/files/miniprofiler-simpledb.png)

Whilst calling the service with the **n1** param yields the following warning:

![Simple N+1 DB Example](http://www.servicestack.net/files/miniprofiler-simpledb-n1.png)

In both cases you see the actual SQL statements performed by clicking the **SQL** link. The N+1 query provides shows the following:

![N+1 DB Example SQL Statementes](http://www.servicestack.net/files/miniprofiler-simpledb-n1-sql.png)

Notice the special attention the MVC MiniProfiler team put into identifying **Duplicate** queries - Thanks Guys!


## Download v3.09

  * **[Using Nuget to add ServiceStack to an existing ASP.NET or MVC application](http://nuget.org/packages/ServiceStack)**
  * [Download ServiceStack.Examples projects and Starter Templates](https://github.com/ServiceStack/ServiceStack.Examples/downloads)
  * [Download just the ServiceStack.dlls binaries](https://github.com/ServiceStack/ServiceStack/downloads)
  * [Other ServiceStack projects available on NuGet](http://www.servicestack.net/docs/framework/nuget)

.

Follow [@demisbellot](http://twitter.com/demisbellot) and [@ServiceStack](http://twitter.com/ServiceStack) for twitter updates

*****

##ServiceStack 2.28 Release Notes

This release includes a few enhancements and catches up on all the pull requests and most of the issues that were submitted 
since the last release.

## ServiceStack is now using Trello.com for features/issue tracking
ServiceStack now hosts and tracks its new issues and feature requests on a 
[live public Trello dash board](https://trello.com/board/servicestack-features-bugs/4e9fbbc91065f8e9c805641c) where anyone 
is welcome to add to, or simply check the progress of their features/issues in the work queue.

## Special Thanks to Contributors
We now have a special [contributor page](https://github.com/ServiceStack/ServiceStack/blob/master/CONTRIBUTORS.md) 
and section on the [main project page](https://github.com/ServiceStack/ServiceStack) showing the many contributors to ServiceStack's 
projects over the years. We hope we haven't missed anyone out - please send us a pull request if you would like to be added.

The major features in this release include:

## Redis MQ Client/Server
A redis-based message queue client/server that can be hosted in any .NET or ASP.NET application. The **RedisMqHost** lives in the
[ServiceStack.Redis](https://github.com/ServiceStack/ServiceStack.Redis) project and brings the many benefits of using a Message Queue. 
The current unoptimized version uses only a single background thread although initial benchmarks shows it can
send/receive a promising **4.6k messages /sec** when accessing a local redis instance (on my dev workstation). 

Major kudos goes to [Redis](http://redis.io) which thanks to its versatility, has Pub/Sub and Lists primitives that makes implementing a Queue trivial.

The first version already sports the major features you've come to expect from a MQ: 

  - Each service maintains its own Standard and Priority MQ's
  - Automatic Retries on messages generating errors with Failed messages sent to a DLQ (Dead Letter Queue) when its Retry threshold is reached.
  - Each message can have a ReplyTo pointing to any Queue, alternatively you can even provide a ServiceStack endpoint URL which will 
    send the response to a Web Service instead. If the web service is not available it falls back into publishing it in the default 
    Response Queue so you never lose a message!
  - MQ/Web Services that don't return any output have their Request DTOs sent to a rolling **Out** queue which can be monitored 
    by external services (i.e. the publisher/callee) to determine when the request has been processed.

Although you can host **RedisMqHost** in any ASP.NET web app, the benefit of hosting inside ServiceStack is that your 
**web services are already capaable** of processing Redis MQ messages **without any changes required** since they're already effectively 
designed to work like a Message service to begin with, i.e. **C# POCO-in -> C# POCO-out**. 

This is another example of ServiceStack's prescribed DTO-first architecture continues to pay dividends since each web service is a DI clean-room 
allowing your **C# logic to be kept pure** as it only has to deal with untainted POCO DTOs, allowing your same web service to be re-used in: 
SOAP, REST (JSON,XML,JSV,CSV,HTML) web services, view models for dynamic HTML pages and now as a MQ service!

Eventually (based on feedback) there will be posts/documentation/examples forthcoming covering how to use it, in the meantime
you can [Check out the Messaging API](https://github.com/ServiceStack/ServiceStack/blob/master/src/ServiceStack.Interfaces/Messaging/IMessageService.cs)
to see how simple it is to use. To see some some working code showing some of the capabilities listed above, 
[view the tests](https://github.com/ServiceStack/ServiceStack.Redis/blob/master/tests/ServiceStack.Redis.Tests/RedisMqHostTests.cs).

Hooking up a basic send/reply example is as easy as:

    //DTO messages:
    public class Hello { public string Name { get; set; } }
    public class HelloResponse { public string Result { get; set; } }

    var redisFactory = new PooledRedisClientManager("localhost:6379");
    var mqHost = new RedisMqHost(redisFactory, noOfRetries:2, null);

    //Server - MQ Service Impl:
    mqHost.RegisterHandler<Hello>(m =>
        new HelloResponse { Result = "Hello, " + m.GetBody().Name });
    mqHost.Start();

    ...

    //Client - Process Response:
    mqHost.RegisterHandler<HelloResponse>(m => {
        Consle.Log("Received: " + m.GetBody().Result);
    });
    mqHost.Start();

    ...
    
    //Producer - Start publishing messages:
    var mqClient = mqHost.CreateMessageQueueClient();
    mqClient.Publish(new Hello { Name = "ServiceStack" });


## JSON/JSV Serializers now supports Abstract/Interface and object types
We're happy to report the most requested feature for ServiceStack's JSON/JSV serializers is now available at:
[ServiceStack.Text v2.28](https://github.com/ServiceStack/ServiceStack.Text/downloads). 

The JSON and JSV Text serializers now support serializing and deserializing DTOs with **Interface / Abstract or object types**.
Amongst other things, this allows you to have an IInterface property which when serialized will include its concrete type information in a 
**__type** property field (similar to other JSON serializers) which when serialized populates an instance of that 
concrete type (provided it exists). 

Likewise you can also have polymorhic lists e.g. of a base **Animal** type and be populated with a **Cats** and **Dogs** which should now deserialize correctly.

As always performance was a primary objective when adding this feature and as a result we should have a very peformant implementation of it.

Note: This feature is **automatically** added to all **Abstract/Interface/Object** types, i.e. you **don't** need to include any 
`[KnownType]` attributes to take advantage of it.

## Other features/fixes:

  - Added JSON/JSV custom serialization behaviour injection of BCL value types in e.g: [JsConfig](https://github.com/ServiceStack/ServiceStack.Text/blob/master/src/ServiceStack.Text/JsConfig.cs#L16)
  - Serialization errors now return 400 status code
  - Add option to propagate errors instead of being sent in the response [Tymek Majewski](https://github.com/letssellsomebananas)
  - Added UserAgent to IHttpRequest type
  - Add useful overloads to HttpResult class
    - SetPermantentCookie/SetSessionCookie/SetCookie
    - LastModified 
  - Fix compression bug in `RequestContext.ToOptimizedResult()`
  - byte[] responses are written directly to the response stream with the ContentType: application/octet-stream

*****

##ServiceStack 2.20 Release Notes

### New Markdown Razor View Engine
The biggest feature in this release is the new Markdown support built-into ServiceStack and more
specifically its **Markdown Razor View Engine**. Markdown Razor is an MVC Razor-inspired templating 
engine that allows you to generate dynamic Markdown and HTML using plain Markdown and Razor Sytnax. 

View the new [Markdown Razor Introduction](http://www.servicestack.net/docs/markdown/markdown-razor) 
for more information.

### ServiceStack.Docs Website Released
The first website to take advantage of the new Markdown templating support in ServiceStack is
**[http://www.servicestack.net/docs](http://www.servicestack.net/docs)** which is effectively built entirely
using ServiceStack's GitHub project Markdown wiki and README.md pages. To render the entire website
the transformed Markdown content is merged with a static **default.shtml** website template.

A nice feature of a Markdown-enabled website is that since the Content is decoupled from the website
template we are easily able to enhance the site using Ajax to load partial content page loads. This
provides a faster browsing experience since the entire webpage doesn't have to be reloaded.

See the [About ServiceStack Docs Website](http://www.servicestack.net/docs/markdown/about) for more 
information.

### MonoTouch support in ServiceStack C# Clients
Support was added to the Generic JSON and JSV ServiceStack C# Clients to work around MonoTouch's
No-JIT Restrictions. Unfortunately to do this we've had to create a new MonoTouch Build 
configuration which doesn't use any C# Expressions or Reflection.Emit. So you need to download the
MonoTouch ServiceStack builds for running in MonoTouch. 
**[Download MonoTouch-v2.20.zip](https://github.com/ServiceStack/ServiceStack/tree/master/release/latest/MonoTouch)**

An example MonoTouch project that uses these Sync and Async C# ServiceClients to talk to the 
[RestFiles](www.servicestack.net/RestFiles/) web services is in the 
[RestFilesClient Example project](https://github.com/ServiceStack/ServiceStack.Examples/tree/master/src/MonoTouch/RestFilesClient).

## Other Features

  - Added support for [IContainerAdapter](https://github.com/ServiceStack/ServiceStack/blob/master/src/ServiceStack.Interfaces/Configuration/IContainerAdapter.cs) to let you [plug-in and use different IOC Containers](https://groups.google.com/d/topic/servicestack/A-W9scHaEBA/discussion)
  - Allow alternate strategies [for resolving Service Types](https://groups.google.com/d/topic/servicestack/Sb7Rcnhte-E/discussion)
  - If your IService implements IDisposable, it will be disposed straight after it's been executed.


#ServiceStack 2.09 Release Notes

## ServiceStack is now on NuGet!

As we have received a number of requests to provide NuGet packages for ServiceStack and its components, we're now happy to say we're now NuGet compliant! Where a configured and working ServiceStack web framework is just 1 NuGet command away :)

[![Install-Pacakage ServiceStack](http://servicestack.net/img/nuget-servicestack.png)](~/framework/nuget)

This will add the ServiceStack dlls to your standard VS.NET ASP.NET Web Application, Register ServiceStack handler in your Web.Config, configure your AppHost and create both a **[Hello](http://servicestack.net/ServiceStack.Hello/)** and a fully-operational **[TODO REST service](http://servicestack.net/Backbone.Todos/)**.

Together with just 2 static content files ([default.htm](https://github.com/ServiceStack/ServiceStack/blob/master/NuGet/ServiceStack/content/default.htm) and [jqunback-1.51.js](https://github.com/AjaxStack/AjaxStack)) you get a fully configured and working REST-ful application (*which as an aside benefit we hope encourages .NET developers into the [beautiful world of Backbone.js](http://documentcloud.github.com/backbone/) and Single Page Ajax Applications*).

The NuGet package of ServiceStack is essentially the **RootPath** Starter Template. The other starting templates, e.g. Windows Service, Console Hosts, hosting ServiceStack at custom /api paths are still available in the [ServiceStack.Examples downloads](https://github.com/ServiceStack/ServiceStack.Examples/downloads).

Check **[ServiceStack's NuGet page](~/framework/nuget)** for the full description of the available ServiceStack packages on NuGet.org

## ServiceStack Overview and Create REST services slides released!

Although this normally shouldn't warrant a release line item, for the technology focused - it's actually hard work :)
We believe the overview slides provide the best starting point for new developers looking to find out the benefits of ServiceStack and how they can easily develop REST services with it. Today, we're releasing the following 2 slides:

### [ServiceStack Overview and Features](https://docs.google.com/present/view?id=dg3mcfb_208gv3kcnd8)
[![Install-Pacakage ServiceStack](http://servicestack.net/img/slides-01-overview-300.png)](https://docs.google.com/present/view?id=dg3mcfb_208gv3kcnd8)

### [Creating REST Web Services](https://docs.google.com/present/view?id=dg3mcfb_213gsvvmmfk)
[![Install-Pacakage ServiceStack](http://servicestack.net/img/slides-02-create-rest-service-300.png)](https://docs.google.com/present/view?id=dg3mcfb_213gsvvmmfk)

## Better configuration

### ASP.NET MVC-like Route API to define user-defined REST paths

In your AppHost Configure() script you can register paths against your Request DTOs with the **Routes** property like so:

  Routes
    .Add<Hello>("/hello")
    .Add<Hello>("/hello/{Name*}") 
    .Add<Todo>("/todos")
    .Add<Todo>("/todos/{Id}");    
    
This is an alternative to the `[Route("/hello")]` attribute which was previously required on your Request DTOs.
They should work as expected, where any match will route that request to the designated service. All variables enclosed with `{Id}` will be populated on the selected Request DTO with that value of the path component. 

`Routes.Add<Hello>("/hello/{Name*}")` is a special case that matches every path beginning with **/hello/** where the **Hello** Request DTOs **Name** property is populated with the contents of the remaining url path. E.g. /hello/**any/path/here** will be populated in Hello.`{Name}` property.

### Disable ServiceStack-wide features

Sometimes the ServiceStack default of having all endpoints and formats all wired up correctly without any configuration is actually not preferred (we know, enterprises right? :), so in this release we've made it easy to turn on and off system-wide features using simple enum flags. To simplify configuration we also added some useful Enum extensions (Has,Is,Add,Remove) to make it easier to signal your intent with Enums. 

E.g. this is how you would disable 'JSV' and 'SOAP 1.1 & 1.2' endpoints:

  var disableFeatures = Feature.Jsv | Feature.Soap;
  SetConfig(new EndpointHostConfig
  {
      EnableFeatures = Feature.All.Remove(disableFeatures),
  });


*****

##ServiceStack 2.08 - ServiceStack meets Backbone.js

Unlike in previous releases, the ServiceStack framework itself has largely remained unchanged. This update is focused towards including [Backbone.js](http://documentcloud.github.com/backbone/) into ServiceStack.Examples project. 

[Backbone.js](http://documentcloud.github.com/backbone/) is a beautifully-designed and elegant light-weight JavaScript framework that allows you to build you're ajax applications separated into **Views** and **Models** connected via key-value data-binding and declarative custom event handling. Of special interest to us is its ability to supply a url and have it **automatically connect your Models** with your **Backend REST services**, which we're happy to report works well with ServiceStack's JSON services.

From the [author](http://twitter.com/jashkenas) of the popular and game-changing libraries [CoffeeScript](http://jashkenas.github.com/coffee-script/) and [Underscore.js](http://documentcloud.github.com/underscore/) - Backbone.js differentiates itself from other javascript frameworks in that it promotes a clean separation of concerns and a modular application design, which is hard to achieve with other frameworks that couple themselves too tightly with the DOM. 

## Backbone's TODO

Our first action was porting Backbone's example TODO app and replace its HTML5 localStorage backend with a ServiceStack REST + Redis one. This was quite easy to do and we were happy that [resulting C# server code for the REST backend](https://github.com/ServiceStack/ServiceStack.Examples/blob/master/src/Backbone.Todos/Global.asax.cs) ended up weighing in at less than the size of VS.NET's default Web.config file :)

Like the rest of our examples a **[live demo is available](http://servicestack.net/Backbone.Todos/)**.

### TODO app now in all Starter templates

As the Backbone TODO app represented a small, but working REST client and server we decided to make it the default app in **all** of ServiceStack's Starter Templates. 

That's right, your Starting template for your **Enterprise Windows Service now comes with a useful TODO app out-of-the-box!** We rightfully believe this makes it the coolest app provided in any starting project template :)

## ServiceStack was built to serve Ajax applications

At this point it's a good time to re-iterate that ServiceStack was designed from the start to be a first-class Ajax server that provides best support for HTML5 Ajax/SPA apps, purely because we believe it to be the future application delivery platform that provides the broadest reach and best user experience possible. We've made special efforts to provide the [fastest JSON web services possible for .NET](http://www.servicestack.net/mythz_blog/?p=344), with a [first-class redis client](~/redis-client/redis-client) and a [strong caching story](~/framework/caching-options) important in developing high-performance web services and a responsive end user experience.

*****

## ServiceStack 2.07 - Q/A Release - Finding Web.Config Nirvana :)

This release was focused on finding the perfect Web.Config that best allows ServiceStack to work consistently everywhere across all ASP.NET and HttpListener hosts on both .NET and MONO platforms.
A primary goal of ServiceStack is to be able to build web services once and use the same binaries and App .config files to run everywhere in every ASP.NET or HttpListener host on Windows with .NET or on OSX/Linux with MONO.

Since your services are POCO message-based and only need to be implement an `IService<TRequest>` interface your services effectively operate in a *clean-room* [DDD Service](http://en.wikipedia.org/wiki/Domain-driven_design), and have a potential for re-use in a variety of other hosts, i.e. Message Queues, Windows services, Windows applications, etc.

### Running cross-platform
Although the promise of the .NET architecture allows for pure C# applications to run on every .NET platform, the ASP.NET hosts don't always share the same compatibility levels as there are subtle differences in implementation and behaviour amongst the various ASP.NET hosts. 

The only real way of ensuring ServiceStack runs well in each environment is to actually setup an environment on each host with each configuration we want to support. Unfortunately this time-consuming process is a necessary one in order to limit any new regressions from being introduced  as a result of added features.

## New ServiceStack Starter Templates projects released
So with that in mind, included in this release is the 'StartTemplates' solution providing the same Hello World Web service hosted in every supported mode and configurations.
There are now 2 supported modes in which to run ServiceStack:

##### a) Do not require an existing Web Framework - Host from the root path: `/`
##### b) Use ServiceStack with an existing Web Framework - Host web services at a user-defined: `/custompath`

### Starter Template projects

The new StarterTemplates in the [ServiceStack.Examples GitHub project](https://github.com/ServiceStack/ServiceStack.Examples/) provide a good starting template for each supported configuration below:

  * [.NET 3.5](https://github.com/ServiceStack/ServiceStack.Examples/tree/master/src/StarterTemplates/ApiPath35) and [.NET 4.0](https://github.com/ServiceStack/ServiceStack.Examples/tree/master/src/StarterTemplates/CustomPath40) - ASP.NET Custom Path: **/api** 
  * [.NET 3.5](https://github.com/ServiceStack/ServiceStack.Examples/tree/master/src/StarterTemplates/RootPath35) and [.NET 4.0](https://github.com/ServiceStack/ServiceStack.Examples/tree/master/src/StarterTemplates/RootPath40) - ASP.NET Root Path: **/**
  * [Windows Service w/ HttpListener](https://github.com/ServiceStack/ServiceStack.Examples/tree/master/src/StarterTemplates/WinServiceAppHost)
  * [Stand alone Console App Host w/ HttpListener](https://github.com/ServiceStack/ServiceStack.Examples/tree/master/src/StarterTemplates/ConsoleAppHost)

We're happy to report the above configurations are well supported on Windows with .NET visible by the **[Latest Windows Integration Test Reports](http://www.servicestack.net/testreports/2011-03-09_RunReports-Windows.htm)** showing ServiceStack running correctly on IIS 3.5,4.0/WebDev Server 2.0,4.0/Windows Service/Console Application hosts.

To deploy on MONO you can just XCOPY/SFTP the files across as-is (i.e. as compiled with VS.NET) to your Linux or OSX server. In most of the scenarios it works as-is however the integration tests have uncovered a couple of known issues visible in the **[Latest Linux Integration Test Reports](http://www.servicestack.net/testreports/2011-03-15_RunReports-Linux.htm)**.

### Known issues on MONO:

  * Depending on your setup a url with a trailing path '/' will cause Nginx/FastCGI or Apache/mod_mono to request a /default.aspx which if it doesn't exist will return a 404
  * If you want to use a custom path i.e. **/api** your ASP.NET virtual path also needs to start with your httpHandler path. Which is why an ASP.NET application hosted on **/ApiPath35/api** works as expected whilst one at **/CustomPath35/api** does not.
  

## Eating our own dogfood 
### ServiceStack.NET now running example projects on both Nginx/FastCGI and Apache/mod_mono on the same server 
  
With a few linux admin tweaks to add and assign a new virtual network interface with a new IP Address, we're easily able to run both Nginx/FastCGI and Apache/mod_mono HTTP servers on the same server, both configured to point to ServiceStack ASP.NET GitHub Example Projects.

[[http://www.servicestack.net]] is running Nginx/FastCGI configuration while the sub domain [[http://api.servicestack.net]] is running Apache/mod_mono (the recommended MONO ASP.NET configuration).

Here are links to ServiceStack.Example projects on both Nginx and Apache:

  * [Nginx](http://www.servicestack.net/ServiceStack.Hello/) - [Apache](http://api.servicestack.net/ServiceStack.Hello/)         /ServiceStack.Hello
  * [Nginx](http://www.servicestack.net/RestFiles/)          - [Apache](http://api.servicestack.net/RestFiles/)                  /RestFiles/           
  * [Nginx](http://www.servicestack.net/RedisStackOverflow/) - [Apache](http://api.servicestack.net/RedisStackOverflow/)         /RedisStackOverflow/
  * [Nginx](http://www.servicestack.net/RedisStackOverflow/) - [Apache](http://api.servicestack.net/RedisStackOverflow/)         /RedisStackOverflow/
  * [Nginx](http://www.servicestack.net/ServiceStack.MovieRest/) - [Apache](http://api.servicestack.net/ServiceStack.MovieRest/) /ServiceStack.MovieRest/
  * [Nginx](http://www.servicestack.net/ServiceStack./) - [Apache](http://api.servicestack.net/ServiceStack.Northwind/)          /ServiceStack.Northwind/

We plan to create more wiki pages walking through how to setup your own ASP.NET web applications on Linux with MONO. 

If you have a preference on what hosting environment you would like to see ServiceStack running in (e.g. AppHarbor, Moncai, Amazon, Azure, SuseStudio, etc), we'd love to hear from you, please post your preference to [ServiceStack's Google Group](http://groups.google.com/group/servicestack)

*****

## ServiceStack v2.0

The ServiceStack code-base has gone under a re-structure to better support user contributions, testing, fine-grained deployments allowing hosting of ServiceStack in 32 and 64 bit servers, in medium or full trust hosting environments.

The changes from a high-level are:

  * No more ILMERGE.exe dlls, all ServiceStack .dlls now map 1:1 with a project of the same name
    * As a result all .pdb's for all assemblies are now included in each release to help debugging (this was lost using ILMERGE)
    * When not using OrmLite/Sqlite, ServiceStack is a full .NET managed assembly with no P/Invokes that can run in 32/64 bit hosts
  * All projects upgraded to VS.NET 2010 (min baseline is still .NET 3.5)
  * Non-core, high-level functionality has been moved into a new [ServiceStack.Contrib](~/contrib/servicestack-contrib)

## Breaking Changes
A lot of effort was made to ensure that clients would not be affected i.e. no code-changes should be required. 

As a result of the change to the deployment dlls where previously ServiceStack.dll was an ILMERGED combination of every implementation dll in ServiceStack. You will now need to explicitly reference each dll that you need. 

To help visualize the dependencies between the various components, here is a tree showing which dependencies each project has:

  * [ServiceStack.Text.dll](~/text-serializers/json-csv-jsv-serializers)
  * [ServiceStack.Interfaces.dll](https://github.com/ServiceStack/ServiceStack/tree/master/src/ServiceStack.Interfaces)

      * [ServiceStack.Common.dll](https://github.com/ServiceStack/ServiceStack/tree/master/src/ServiceStack.Common)

        * [ServiceStack.dll](https://github.com/ServiceStack/ServiceStack/tree/master/src/ServiceStack)
          * [ServiceStack.ServiceInterface.dll](https://github.com/ServiceStack/ServiceStack.Contrib/tree/master/src/ServiceStack.ServiceInterface)

        * [ServiceStack.Redis.dll](~/redis-client/redis-client)
        * [ServiceStack.OrmLite.dll](~/ormlite/ormlite-overview)


### Non-core Framework features extracted into new ServiceStack.Contrib project

In the interest of promoting contributions and modifications from the community, the non-core projects of ServiceStack has been extracted into a new user contributed **ServiceStack.Contrib** project site at:

**[[https://github.com/ServiceStack/ServiceStack.Contrib]]**

I invite all ServiceStack users who want to share their generic high-level functionality and useful app-specific classes under this project where the rest of the community can benefit from.

*****

## Service Stack 1.82 Release Notes

### [New HTML5 Report Format Added](~/framework/json-report-format)

The biggest feature added in this release is likely the new HTML5 report format that generates a human-readable HTML view of your web services response when viewing it in a web browser.
Good news is, like the [[ServiceStack-CSV-Format]] it works with your existing webservices as-is, with no configuration or code-changes required.
  
[![HTML5 Report Format](http://servicestack.net/img/HTML5Format.png)](~/framework/json-report-format)

Here are some results of web services created before the newer HTML5 and CSV formats existed:

  * **RedisStackOverflow** [Latest Questions](http://servicestack.net/RedisStackOverflow/questions)
  * **RestMovies** [All Movie listings](http://servicestack.net/ServiceStack.MovieRest/movies)
  * **RestFiles** [Root Directory](http://servicestack.net/RestFiles/files)

Use the **?format=[json|xml|html|csv|jsv]** to toggle and view the same webservice in different formats.

### New ServiceStack.Northwind Example project added

In order to be able to better demonstrate features with a 'real-world' DataSet, a new ServiceStack.Northwind project has been added which inspects the Northwind dataset from an SQLite database.
A live demo is hosted at [[http://servicestack.net/ServiceStack.Northwind/]]. Here are some links below to better demonstrate the new HTML format with a real-world dataset:

#### Nortwind Database REST web services
  * [All Customers](http://servicestack.net/ServiceStack.Northwind/customers) 
  * [Customer Detail](http://servicestack.net/ServiceStack.Northwind/customers/ALFKI)
  * [Customer Orders](http://servicestack.net/ServiceStack.Northwind/customers/ALFKI/orders)


### Improved Caching

ServiceStack has always had its own (i.e. ASP.NET implementation-free) [good support for caching](~/framework/caching-options), though like most un-documented features it is rarely used. The caching has been improved in this version to now support caching of user-defined formats as well. Here is example usage from the new Northwind project:

    public class CachedCustomersService : RestServiceBase<CachedCustomers>
    {
        public ICacheClient CacheClient { get; set; }

        public override object OnGet(CachedCustomers request)
        {
            return base.RequestContext.ToOptimizedResultUsingCache(
                this.CacheClient, "urn:customers", () => {
                    var service = base.ResolveService<CustomersService>();
                        return (CustomersResponse) service.Get(new Customers());
                });
        }
    }

The above code caches the most optimal output based on browser capabilities, i.e. if your browser supports deflate compression (as most do), a deflated, serialized output is cached and written directly on the response stream for subsequent calls. Only if no cache exists will the web service implementation (e.g lambda) be executed, which populates the cache before returning the response.

To see the difference caching provides, here are cached equivalents of the above REST web service calls:

#### Nortwind Database **Cached** REST web services
  * [All Customers](http://servicestack.net/ServiceStack.Northwind/cached/customers) 
  * [Customer Detail](http://servicestack.net/ServiceStack.Northwind/cached/customers/ALFKI)
  * [Customer Orders](http://servicestack.net/ServiceStack.Northwind/cached/customers/ALFKI/orders)


### API Changes

The underlying IHttpRequest (an adapter interface over ASP.NET/HttpListener HTTP Requests) can now be retrieved within your webservice to be able to query the different HTTP Request properties:

    var httpReq = base.RequestContext.Get<IHttpRequest>();
    
Also added is the ability to resolve existing web services (already auto-wired by the IOC) so you can re-use existing web service logic. Here is an example of usage from the Northwind [CustomerDetailsService.cs](https://github.com/ServiceStack/ServiceStack.Examples/blob/master/src/ServiceStack.Northwind/ServiceStack.Northwind.ServiceInterface/CustomerDetailsService.cs).

    var ordersService = base.ResolveService<OrdersService>();
    var ordersResponse = (OrdersResponse)ordersService.Get(new Orders { CustomerId = customer.Id });


## Service Stack 1.79 Release Notes

### The C#/.NET Sync and Async Service Clients were improved to include: 
  * Enhanced REST functionality and access, now more succinct than ever
  * Uploading of files to ServiceStack web services using **HTTP POST** *multipart/form-data*
  * More robust error handling support handling C# exceptions over REST services 
  * For examples of on how to use the C# REST client API check out the tests in the new REST Files project:
    * [Sync C# client examples](https://github.com/ServiceStack/ServiceStack.Examples/blob/master/src/RestFiles/RestFiles.Tests/SyncRestClientTests.cs) 
    * [Async C# client examples](https://github.com/ServiceStack/ServiceStack.Examples/blob/master/src/RestFiles/RestFiles.Tests/AsyncRestClientTests.cs)

## New RestFiles project added to [ServiceStack.Examples](https://github.com/ServiceStack/ServiceStack.Examples/) GitHub project:
#### Live demo available at: [servicestack.net/RestFiles/](http://servicestack.net/RestFiles/)

  * Provides a complete remote file system management over a [REST-ful api](http://servicestack.net/RestFiles/servicestack/metadata) 
  * The complete REST-ful /files web service implementation is only [**1 C# page class**](https://github.com/ServiceStack/ServiceStack.Examples/blob/master/src/RestFiles/RestFiles.ServiceInterface/FilesService.cs)
  * Includes a pure ajax client to provide a **GitHub-like** file browsing experience, written in only [**1 static HTML page, using only jQuery**](https://github.com/ServiceStack/ServiceStack.Examples/blob/master/src/RestFiles/RestFiles/default.htm)
   * [C# integration test examples](https://github.com/ServiceStack/ServiceStack.Examples/blob/master/src/RestFiles/RestFiles.Tests/) are also included showing how to access this REST-ful api over sync and async C# clients

Read the rest of the [Rest Files README.md](https://github.com/ServiceStack/ServiceStack.Examples/tree/master/src/RestFiles/RestFiles) for a more detailed overview about the project.


## Service Stack 1.78 Release Notes

 * Added more tests and fixed bugs in ServiceStack's new CSV format and Request/Response filters
 * Added new information on the generated web service index, individual web service page now include:
   * REST paths (if any are defined) thanks to [@jakescott](http://twitter.com/jakescott)
   * Included directions to consumers on how to override the HTTP **Accept** header and specify the **format**
   * Now including any System.CompontentModel.**Description** meta information attributed on your Request DTO
   * Preview the new documentation pages on ServiceStack [**Hello**](http://www.servicestack.net/ServiceStack.Hello/servicestack/json/metadata?op=Hello) and [**Movies**](http://www.servicestack.net/ServiceStack.MovieRest/servicestack/xml/metadata?op=Movie) example web service pages.
 * Added [tests to show how to implement Basic Authentication](https://github.com/ServiceStack/ServiceStack/blob/master/tests/ServiceStack.WebHost.Endpoints.Tests/RequestFiltersTests.cs) using the new RequestFilters
 * Changed the httpHandler paths in the Example projects and [created a new Config class](https://github.com/ServiceStack/ServiceStack/blob/master/src/ServiceStack.WebHost.Endpoints/SupportedHandlerMappings.cs) to store which supported mappings go with which web servers + middleware.
 * Provide a way to register new urls for different ServiceStack handler mappings used, e.g. to register IIS 6.0 urls:

       SetConfig(new EndpointConfig { ServiceEndpointsMetadataConfig = ServiceEndpointsMetadataConfig.GetForIis6ServiceStackAshx() });


## Service Stack 1.77 Release Notes

This release was focused to opening up ServiceStack to better support adding more hooks and extension points where new formats can be added. The CSV format was also added to test these new extension APIs.

## Main features added in this release:

* Added support for the [CSV format](~/framework/csv-format)
* Enhanced the IContentTypeFilter API to add support for different serialization formats
* Added Request and Response filters so custom code can inspect and modify the incoming [IHttpRequest](https://github.com/ServiceStack/ServiceStack/blob/master/src/ServiceStack.ServiceHost/IHttpRequest.cs) or [IHttpResponse](https://github.com/ServiceStack/ServiceStack/blob/master/src/ServiceStack.ServiceHost/IHttpResponse.cs). 
* Added `Request.Items` so you can share arbitrary data between your filters and web services.
* Added `Request.Cookies` for reading cookies (to avoid retrieving it from HttpRuntime.Current)
* Removed the preceding UTF8 BOM character to ServiceStack's JSON and JSV Serializers. 
* All features above are available on both ASP.NET and HttpListener hosts

### [CSV Format](~/framework/csv-format)

Using the same tech that makes [ServiceStack's JSV and JSON serializers so fast](http://www.servicestack.net/benchmarks/NorthwindDatabaseRowsSerialization.100000-times.2010-08-17.html) (i.e. no run-time reflection, static delegate caching, etc), should make it the fastest POCO CSV Serializer available for .NET.

The 'CSV' format is the first format added using the new extensions API, which only took the following lines of code:

  //Register the 'text/csv' content-type and serializers (format is inferred from the last part of the content-type)
  this.ContentTypeFilters.Register(ContentType.Csv,
    CsvSerializer.SerializeToStream, CsvSerializer.DeserializeFromStream);

  //Add a response filter to add a 'Content-Disposition' header so browsers treat it as a native .csv file
  this.ResponseFilters.Add((req, res, dto) =>
    {
      if (req.ResponseContentType == ContentType.Csv)
      {
        res.AddHeader(HttpHeaders.ContentDisposition,
          string.Format("attachment;filename={0}.csv", req.OperationName));
      }
    });

With only the code above, the 'CSV' format is now a first-class supported format which means all your existing web services can take advantage of the new format without any config or code changes. Just drop the latest ServiceStack.dlls (v1.77+) and you're good to go! 

Note: there are some limitations on the CSV format and implementation which you can read about on the [ServiceStack CSV Format page](~/framework/csv-format).

### Request and Response Filters:

The Request filter takes a IHttpRequest, IHttpResponse and the **Request DTO**:
    List<Action<IHttpRequest, IHttpResponse, object>> RequestFilters { get; }

The Response filter takes a IHttpRequest, IHttpResponse and the **Response DTO**:
    List<Action<IHttpRequest, IHttpResponse, object>> ResponseFilters{ get; }

Note: both sets of filters are called before there any output is written to the response stream so you can happily use the filters to authorize and redirect the request. Calling `IHttpResponse.Close()` will close the response stream and stop any further processing of this request.

Feel free to discuss or find more about any of these features at the [Service Stack Google Group](https://groups.google.com/forum/#!forum/servicestack)


[<Wiki Home](~/framework/home)