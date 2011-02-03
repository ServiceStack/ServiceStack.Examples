#The REST Files

The Rest files is an example ServiceStack project that provides remote access of your filesystem, 
over REST-ful web services, in a GitHub browser-like widget.

## Client Info

The client is written in [1 static default.html page, using only jQuery](https://github.com/mythz/ServiceStack.Examples/blob/master/src/RestFiles/RestFiles/default.htm)
Because of the advanced HTML5 features used its best used in a modern browser (i.e. anything but IE)

  - CSS3 is used for foler animations
  - HTML5 History State API is used for page navigation

## Server Info

The entire /files service exposes a REST-ful strong-typed API written using only
[1 single C# class](https://github.com/mythz/ServiceStack.Examples/blob/master/src/RestFiles/RestFiles.ServiceInterface/FilesService.cs).

As it was developed using the http://servicestack.net Open Source .NET/Mono Web Services Framework
it also able to expose this REST-ful API over a myraid of formats (with no extra code/config):

  json - http://localhost/RestFiles/files/dtos/Types?format=json
  xml  - http://localhost/RestFiles/files/dtos/Types?format=xml
  jsv  - http://localhost/RestFiles/files/dtos/Types?format=jsv&debug=true
  csv  - http://localhost/RestFiles/files/dtos/Types?format=csv

*Note: All but the xml format uses the high-performance cross-platform, serializers in
ServiceStack.Text. The JsonSerializer serializer is over 3.6x faster that the fastest JSON Serialzer 
shipped with .NET, see:
http://www.servicestack.net/benchmarks/NorthwindDatabaseRowsSerialization.100000-times.2010-08-17.html

SOAP 1.1/1.2 endpoints are also available at the following url:

  soap11 - http://localhost/RestFiles/servicestack/soap11
  soap12 - http://localhost/RestFiles/servicestack/soap12

As a result of the strong-typed DTO pattern used to define the the webservice, ServiceStack is able to
generate the xsds, wsdls, metadata documentation on the fly at:

  docs - http://localhost/RestFiles/servicestack/metadata
  xsd  - http://localhost/RestFiles/servicestack/metadata?xsd=1
  wsdl - http://localhost/RestFiles/servicestack/soap12 (HTTP GET)


## Live Demo

The live demo is hosted on Linux (Cent OS) / Nginx using [MONO](http://www.mono-project.com)

*Not affiliated with GitHub, which is a trademark of GitHub Inc