# Access the HTTP Request in Web Services

By default ServiceStack provides a clean, dependency-free IService<T> to implement your Web Services logic in. The philosophy behind this approach is that the less dependencies you have on your environment and its request context, the more testable and re-usable your services become. 

### Advantages for living without it
If you don't need to access the HTTP Request context there is nothing stopping you from having your same IService<T> implementation processing requests from a message queue which we've done for internal projects (which incidentally is the motivation behind the **asynconeway** endpoint, to signal requests that are safe for deferred execution).

## Injecting the IRequestContext into your Service
Although working in a clean-room can be ideal ideal from re-usability and testability point of view, you stand the chance of missing out a lot of the features present in HTTP. So not wanting to limit your web services usefulness, this feature of accessing the RequestContext has been present in ServiceStack from early on however because it hasn't been well documented its not very well-known (An issue this wiki page hopes to correct :)

Just like using built-in Funq IOC container, the way to tell ServiceStack to inject the request context is by implementing the [IRequiresRequestContext](https://github.com/ServiceStack/ServiceStack.Interfaces/blob/master/src/ServiceStack.ServiceHost/IRequiresRequestContext.cs) interface which will get the [IRequestContext](https://github.com/ServiceStack/ServiceStack.Interfaces/blob/master/src/ServiceStack.ServiceHost/IRequestContext.cs) inject before each request.

	public interface IRequestContext : IDisposable
	{
		T Get<T>() where T : class;

		string IpAddress { get; }

		IDictionary<string, Cookie> Cookies { get; }

		EndpointAttributes EndpointAttributes { get; }

		IRequestAttributes RequestAttributes { get; }

		string MimeType { get; }

		string CompressionType { get; }

		string AbsoluteUri { get; }

		IFile[] Files { get; }
	}


This will allow your services to inspect any Cookies or download any Files that were sent with the request.
Note: to set Response Cookies or Headers, return the [HttpResult](https://github.com/ServiceStack/ServiceStack/blob/master/src/ServiceStack.Common/Web/HttpResult.cs) object.

## Accessing the IHttpRequest and IHttpResponse using filters
A recent addition to ServiceStack is the ability to register custom Request and Response filters. These should be registered in your AppHost.Configure() onload script: 

* The Request Filters are applied before the service gets called and accepts:
_([IHttpRequest](https://github.com/ServiceStack/ServiceStack/blob/master/src/ServiceStack.ServiceHost/IHttpRequest.cs), [IHttpResponse](https://github.com/ServiceStack/ServiceStack/blob/master/src/ServiceStack.ServiceHost/IHttpResponse.cs), RequestDto)_ e.g:
    
	    //Add a request filter to check if the user has a session initialized
	    this.RequestFilters.Add((httpReq, httpReq, requestDto) =>
	    {
			var sessionId = httpReq.GetCookieValue("user-session");
			if (sessionId == null)
			{
				httpReq.ReturnAuthRequired();
			}
	    });
    

* The Response Filters are applied after your service is called and accepts:
_([IHttpRequest](https://github.com/ServiceStack/ServiceStack/blob/master/src/ServiceStack.ServiceHost/IHttpRequest.cs), [IHttpResponse](https://github.com/ServiceStack/ServiceStack/blob/master/src/ServiceStack.ServiceHost/IHttpResponse.cs), ResponseDto)_ e.g:

	    //Add a response filter to add a 'Content-Disposition' header so browsers treat it as a native .csv file
	    this.ResponseFilters.Add((req, res, dto) =>
	    {
			if (req.ResponseContentType == ContentType.Csv)
			{
			    res.AddHeader(HttpHeaders.ContentDisposition,
				string.Format("attachment;filename={0}.csv", req.OperationName));
			}
	    });


[<Wiki Home](~/framework/home)