A summary of Service Stack's available and recommended Web Service end points.

# Available web service endpoints 
As different endpoints have advantages in different scenarios, Service Stack supports the following endpoints out of the box:

  * XML  (+ REST GET urls) - Faster than JSON. Most standards compliant and interoperable format (Most Recommended).
  * JSON (+ REST GET urls) - More compact but slower than XML (Recommended for use in Ajax applications).
  * JSV  (+ REST GET urls) - Fast, compact, resilient serialization format (Recommended for .NET to .NET web services).
  * SOAP 1.1/1.2 - Utilizes WCF SOAP web services under the covers. (Recommended for corporate Intranet environments where maximum performance is not a priority or if you have a preference WSDL tooling support e.g. IDE integration and auto-generated client proxies, etc).

If you're interested for a more background information on the history of the different formats and the advantages and disadvantages of each read on below...

# Service Stack's view on the history of Web Services 

The W3C defines a "web service" as "a software system designed to support interoperable machine-to-machine interaction over a network.

The key parts of this definition are that it should be interoperable and that it facilitates communication over a network. Unfortunately over the years different companies have had different ideas on what the most ideal interoperable protocol should be, leaving a debt-load of legacy binary and proprietary protocols in its wake.

## HTTP the de facto web services transport protocol
HTTP the Internet's protocol is the undisputed champ and will be for the foreseeable future. It's universally accepted, can be proxied and is pretty much the only protocol allowed through most firewalls which is the reason why Service Stack (and most other Web Service frameworks) support it. Note: the future roadmap will also support the more optimized HTML5 'Web Sockets' standard.

## XML the winning serialization format?
Out of the ashes another winning format looking to follow in HTTP's success, is the XML text serialization format. Some of the many reasons why it has reigned supreme include:

  * Simple, Open, self-describing text-based format
  * Human and Computer readable and writeable
  * Verifiable
  * Provides a rich set of common data types
  * Can define higher-level custom types
XML doesn't come without its disadvantages which currently are centered around it being verbose and being slow to parse resulting wasted CPU cycles.

## REST vs. SOAP
Despite the win, all is not well in XML camp. It seems that two teams are at odds looking to branch the way XML is used in web services. On one side, I'll label the REST camp (despite REST being more than just XML) approach to developing web services is centred around resources and prefers to err on simplicity and convention choosing to re-use the other existing HTTP metaphors where they're semantically correct. E.g. calling GET on the URL `http://host/customers` will most likely return a list of customers, whilst PUT'ing a 'Customer' against the same url will, if supported append the 'Customer' to the existing list of customers.

The URL's used in RESTful web services also form a core part of the API, it is normally logically formed and clearly describes the type of data that is expected, e.g. viewing a particular customers order would look something like:

  * GET `http://location/customers/mythz/orders/1001` - would return details about order '1001' which was placed by the customer 'mythz'.
The benefit of using a logical URL scheme is that other parts of your web services API can be inferred, e.g.

  * GET `http://location/customers/mythz/orders` - would return all of 'mythz' orders
  * GET `http://location/customers/mythz` - would return details about the customer 'mythz'
  * GET `http://location/customers` - would return a list of all customers
If supported, you may have access to different operations on the same resources via the other HTTP methods: POST, PUT and DELETE. One of the limitations of having a RESTful web services API is that although the API may be conventional and inferable by humans, it isn't friendly to computers and likely requires another unstructured document accompanying the web services API identifying the list, schema and capabilities of each service. This makes it a hard API to provide rich tooling support for or to be able to generate a programmatic API against.

NOTE: If you're interested in learning more about REST one of the articles I highly recommend is [http://tomayko.com/writings/rest-to-my-wife](http://tomayko.com/writings/rest-to-my-wife)

### Enter SOAP
SOAP school discards this HTTP/URL nonsense and teaches that there is only one true METHOD - the HTTP 'POST' and there is only one url / end point you need to worry about - which depending on the technology chosen would look something like `http://location/CustomerService.svc`. Importantly nothing is left to the human imagination, everything is structured and explicitly defined by the web services WSDL which could be also obtained via a url e.g. `http://location/CustomerService.svc?wsdl`. Now the WSDL is an intimately detailed beast listing everything you would ever want to know about the definition of your web services. Unfortunately it's detailed to the point of being unnecessarily complex where you have layers of artificial constructs named messages, bindings, ports, parts, input and output operations, etc. most of which remains un-utilized which a lot of REST folk would say is too much info that can be achieved with a simple GET request :)

What it does give you however, is a structured list of all the operations available, including the schema of all the custom types each operation accepts. From this document tools can generate a client proxy into your preferred programming language providing a nice strongly-typed API to code against. SOAP is generally favoured by a lot of enterprises for internal web services as in a lot of cases if the code compiles then there's a good chance it will just work.

Ultimately on the wire, SOAP services are simply HTTP POSTs to the same endpoint where each payload (usually of the same as the SOAP-Action) is wrapped inside the body of a 'SOAP' envelope. This layer stops a lot of people from accessing the XML payload directly and have to resort to using a SOAP client library just to access the core data.

This complexity is not stopping the Microsoft's and IBM's behind the SOAP specification any-time soon. Nope they're hard at work finishing their latest creations that are adding additional layers on top of SOAP (i.e. WS-Security, WS-Reliability, WS-Transaction, WS-Addressing) which is commonly referred to as the `WS-*` standards. Interestingly the `WS-*` stack happens to be complex enough that they happen to be the only companies able to supply the complying software and tooling to support it, which funnily enough works seamlessly with their expensive servers.

It does seem that Microsoft, being the fashionable technology company they are don't have all their eggs in the `WS-*` bucket. Realizing the current criticisms on their current technology stack, they have explored a range of other web service technologies namely WCF Data Services, WCF RIA Services and now their current favourite OData. The last of which I expect to see all their previous resource efforts in `WS-*` to be transferred into promoting this new Moniker. On the surface OData seems to be a very good 'enabling technology' that is doing a good job incorporating every good technology BUZZ-word it can (i.e. REST, ATOM, JSON). It is also being promoted as 'clickbox driven development' technology (which I'll be eagerly awaiting to see the sticker for :).

Catering for drag n' drop developers and being able to create web services with a checkbox is a double-edged sword which I believe encourages web service development anti-patterns that run contra to SOA-style (which I will cover in a separate post). Just so everyone knows the latest push behind OData technology is to give you more reasons to use Azure (Microsoft's cloud computing effort).

## POX to the rescue?
For the pragmatic programmer it's becoming a hard task to follow the `WS-*` stack and still be able to get any work done. For what appears to be a growing trend, a lot of developers have taken the best bits from SOAP and WSDL and combined them in what is commonly referred to as POX or REST+POX. Basically this is Plain Old Xml over HTTP and REST-like urls. In this case a lot of the cruft inside a WSDL can be reduced to a simple XSD and a url. The interesting part about POX is that although there seems to be no formal spec published, a lot of accomplished web service developers have ultimately ended up at the same solution. The advantages this has over SOAP are numerous many of which are the same reasons that have made HTTP+XML ubiquitous. It is a lot simpler, smaller and faster at both development and runtime performance - while at the same time retaining a strongly-typed API (which is one of the major benefits of SOAP). Even though it's lacking a formal API, it can be argued that POX is still more interoperable than SOAP as clients no longer require a SOAP client to consume the web service and can access it simply with a standard Web Client and XML parser present in most programming environments, even most browsers.

## And then there was JSON
One of the major complaints of XML is that it's too verbose, which given a large enough dataset consumes a lot of bandwidth. It is also a lot stricter than a lot of people would like and given the potential for an XML document to be composed from many different namespaces and for a type to have both elements and attributes - it is not an ideal fit for most programming models. As a result of this, parsing XML can be quite cumbersome especially inside of a browser. A popular format which is seeking to overcome both of these problems and is now the preferred serialization format for AJAX applications is JSON. It is very simple to parse and maps perfectly to a Java Script object, it is also safe format which is the reason why it's chosen over pure Java Script. It's also a more 'dynamic' and resilient format than XML meaning that adding new or renaming existing elements or their types will not break the de-serialization routine as there is no formal spec to adhere to which is both and advantage and disadvantage. Unfortunately even though it's a smaller, more simple format it is actually deceptively slower to de/serialize than XML using the available .NET libraries based on the available benchmarks. This performance gap is more likely due to the amount of effort Microsoft has put into their XML DataContractSerializer than a deficiency of the format itself as my effort of developing a JSON-like serialization format is both smaller than JSON and faster than XML - the best of both worlds.

## Service Stack's new JSV Format
The latest endpoint to be added to Service Stack, is JSV the serialization format of Service Stack's POCO TypeSerializer. It's a JSON inspired format that uses CSV-style escaping for the least overhead and optimal performance. 

With the interest of creating high-performance web services and not satisfied with the performance or size of existing XML and JSON serialization formats, TypeSerializer was created with a core goal to create the most compact and fastest text-serializer for .NET. In this mission, it has succeeded as it is now both 5.3x quicker than the leading .NET JSON serializer whilst being 2.6x smaller than the equivalent XML format. 

TypeSerializer was developed from experience taking the best features of serialization formats it looks to replace.  It has many other features that sets it apart from existing formats which makes it the best choice for serializing any .NET POCO object. 

  * Fastest and most compact text-serializer for .NET
  * Human readable and writeable, self-describing text format 
  * Non-invasive and configuration-free 
  * Resilient to schema changes (focused on deserializing as much as possible without error)
  * Serializes / De-serializes any .NET data type (by convention) 
    * Supports custom, compact serialization of structs by overriding `ToString()` and `static T Parse(string)` methods 
    * Can serialize inherited, interface or 'late-bound objects' data types 
    * Respects opt-in `DataMember` custom serialization for `DataContract` DTO types.

For these reasons it is the preferred choice to transparently store complex POCO types for OrmLite (in text blobs), POCO objects with 
[Service Stack's C# Redis Client](https://github.com/mythz/ServiceStack.Redis) or the optimal serialization format in .NET to .NET web services.
