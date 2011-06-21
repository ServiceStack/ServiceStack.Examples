# Use ServiceStack's JsonSerializer in ASP.NET MVC

The easiest way to get an instant perf boost when developing JSON services with MVC is to
override MVC's JsonResult to use ServiceStack's JsonSerializer instead of the default slow
JSON serializer shipped with ASP.NET MVC.

Our benchmarks show ServiceStack's JsonSerializer 
**[is over 3x faster than JSON.NET](http://www.servicestack.net/benchmarks/NorthwindDatabaseRowsSerialization.100000-times.2010-08-17.html)**
using Microsoft's sample real-world Northwind database whilst others are reporting similar success:

  - [JSON.NET vs ServiceStack](http://daniel.wertheim.se/2011/02/07/json-net-vs-servicestack/)
  - [C# and spray painting vegetables on ServiceStack.NET](http://fir3pho3nixx.blogspot.com/2011/04/servicestacknet.html)


You can change the default ASP.NET MVCs JsonResult to use ServiceStack's JsonSerializer 
by creating your own base controller that overrides `Json(object,string,Encoding)` with the 
following implementation:

    public abstract class ApplicationController : Controller
    {
        protected ActionResult RespondTo(Action<FormatCollection> format) {
            return new FormatResult(format);
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior) {
            return new ServiceStackJsonResult {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };
        }
    }
    
    public class ServiceStackJsonResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context) {
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null) {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null) {
                response.Write(JsonSerializer.SerializeToString(Data));
            }
        }
    }    
    
Special thanks to [@JakeScott](http://twitter.com/JakeScott) for his 
[gist for the above source code](https://gist.github.com/1037528).