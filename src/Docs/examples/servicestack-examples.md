[Join the new google group](http://groups.google.com/group/servicestack) or
follow [@demisbellot](http://twitter.com/demisbellot) and [@ServiceStack](http://twitter.com/servicestack)
for twitter updates. 

#Example Projects built with [ServiceStack](~/framework/overview), [C# RedisClient](~/redis-client/redis-client), [OrmLite](~/ormlite/ormlite-overview), etc

## Live Demo

A live demo and tutorials are available at the following locations:

### [Backbone.js TODO app with REST and Redis backend](http://mono.servicestack.net/Backbone.Todos/)
[![Backbone REST and Redis TODOs](http://mono.servicestack.net/showcase/img/todos-400x350.png)](http://mono.servicestack.net/Backbone.Todos/)

### [Creating a Hello World Web service from scratch](http://mono.servicestack.net/ServiceStack.Hello/)
[![ServiceStacks Hello, World!](http://mono.servicestack.net/showcase/img/hello-400x350.png)](http://mono.servicestack.net/ServiceStack.Hello/)

### [GitHub-like browser to manage remote filesystem over REST](http://mono.servicestack.net/RestFiles/)
[![GitHub-like REST Files](http://mono.servicestack.net/showcase/img/restfiles-400x350.png)](http://mono.servicestack.net/RestFiles/)

### [Creating a StackOverflow-like app in Redis](http://mono.servicestack.net/RedisStackOverflow/)
[![Redis StackOverflow](http://mono.servicestack.net/showcase/img/redisstackoverflow-400x350.png)](http://mono.servicestack.net/RedisStackOverflow/)

### [Complete REST Web service example](http://mono.servicestack.net/ServiceStack.MovieRest/)
[![REST at the Movies!](http://mono.servicestack.net/showcase/img/movierest-400x350.png)](http://mono.servicestack.net/ServiceStack.MovieRest/)

### [Calling Web Services with Ajax](http://mono.servicestack.net/ServiceStack.Examples.Clients/)
[![Ajax Example](http://mono.servicestack.net/showcase/img/ajaxexample-400x350.png)](http://mono.servicestack.net/ServiceStack.Examples.Clients/)

### Other examples
* [Calling Web Services with Mono Touch](http://www.servicestack.net/monotouch/remote-info/)
* [Calling Web Services using Silverlight](http://mono.servicestack.net/ServiceStack.Examples.Clients/Silverlight.htm)
* [Calling SOAP 1.1 Web Service Examples](http://mono.servicestack.net/ServiceStack.Examples.Clients/Soap11.aspx)
* [Calling SOAP 1.2 Web Service Examples](http://mono.servicestack.net/ServiceStack.Examples.Clients/Soap12.aspx)

_All live examples hosted on CentOS/Nginx/FastCGI/Mono_

# Download

You can find the latest releases for download at:

* [ServiceStack.Examples/downloads](https://github.com/ServiceStack/ServiceStack.Examples/downloads)


### Troubleshooting

- Since the example project uses 32bit Sqlite.dll, on a 64bit machine you must set IIS to run 32bit apps (in the App Domain config)