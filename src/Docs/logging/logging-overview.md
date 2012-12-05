## ServiceStack.Logging an implementation-free logging interface for your app logic to bind to
For twitter updates follow <a href="http://twitter.com/demisbellot">@demisbellot</a> or <a href="http://twitter.com/servicestack">@ServiceStack</a>

Even in the spirit of **Bind to interfaces, not implementations**, many .NET projects still have
a hard dependency to [log4net](http://logging.apache.org/log4net/index.html). 

Although log4net is the standard for logging in .NET, potential problems can arise from your libraries having a hard dependency on it:

* Your library needs to be shipped with a third-party dependency
* Potential conflicts can occur when different libraries have dependency on different versions of log4net (e.g. the 1.2.9 / 1.2.10 dependency problem).
* You may want to use a different logging provider (i.e. network distributed logging)
* You want your logging for Unit and Integration tests to redirect to the Console or Debug logger without any configuration.

ServiceStack.Logging solves these problems by providing an implementation-free ILog interface that your application logic can bind to 
where your Application Host project can bind to the concrete logging implementation at deploy or runtime.

ServiceStack.Logging also includes adapters for the following logging providers:

* Log4Net 1.2.10+
* Log4Net 1.2.9
* EventLog
* Console Log
* Debug Log
* Null / Empty Log

# Usage Examples

Once on your App Startup, either In your AppHost.cs or Global.asax file inject the concrete logging implementation that your app should use, e.g.

## Log4Net
    LogManager.LogFactory = new Log4NetFactory(true); //Also runs log4net.Config.XmlConfigurator.Configure()

## Event Log
    LogManager.LogFactory = new EventLogFactory("ServiceStack.Logging.Tests", "Application");

Then your application logic can bind to and use a lightweight implementation-free ILog which at runtime will be an instance of the concrete implementation configured in your host:

    ILog log = LogManager.GetLogger(GetType());

    log.Debug("Debug Event Log Entry.");
    log.Warn("Warning Event Log Entry.");




