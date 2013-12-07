#New JavaScript HTML5 Report Format
 
##New HTML5 + JSON Report Format - [Released in 1.82](~/framework/release-notes)

These examples are simply links to existing ServiceStack web services, which based on your browsers user-agent (i.e. Accept: 'text/html') provides this HTML format instead of the other serialization formats. 

  - **Northwind Database: **  [All Customers](http://mono.servicestack.net/ServiceStack.Northwind/customers), [Customer Detail](http://mono.servicestack.net/ServiceStack.Northwind/customers/ALFKI), [Customer Orders](http://mono.servicestack.net/ServiceStack.Northwind/orders)
  - **RedisStackOverflow: **  [Latest Questions](http://mono.servicestack.net/RedisStackOverflow/questions) and [Site Stats](http://mono.servicestack.net/RedisStackOverflow/stats)
  - **RestMovies: **  [All Movie listings](http://mono.servicestack.net/ServiceStack.MovieRest/movies)
  - **RestFiles: **  [Root Directory](http://mono.servicestack.net/RestFiles/files)

[![HTML5 Report Format](http://mono.servicestack.net/img/HTML5Format.png)](http://mono.servicestack.net/ServiceStack.Northwind/customers/ALFKI)

To see it in action, **view the source** in your browser. Webkit and Firefox users can simply go to the url below:

    view-source:http://mono.servicestack.net/ServiceStack.Northwind/customers/ALFKI

Note: To view the web services in a different format simply append **?format=[json|xml|html|csv|jsv]** to the query string.


The primary focus for the HTML Format is to provide a readable and semantic HTML layout letting you visualize all the data returned by your web service with a single glance.
Features include:

### Human Friendly output
Based on convention, it generates a recursive and cascading view of your data using a combination of different sized definition lists and tables where appropriate.
After it's rendered convenience behavior is applied allowing you to sort your tabular data, view the embedded JSON contents as well as providing links back to the original web service that generated the report including links to the other formats supported.

### Completely self-contained
The report does not have any external CSS, JavaScript or Images which also helps achieve its super-fast load-time and rendering speed.

### Embeds the complete snapshot of your web services data
The report embeds a complete, unaltered version of your 'JSON web service' capturing a snapshot of the state of your data at a given point in time. 
It's perfect for backups with the same document containing a human and programmatic access to the data. 
The JSON data is embedded inside a valid and well-formed document, making it programmatically accessible using a standard XML/HTML parser. 
The report also includes an interface to allow humans to copy it from a textbox.
  
### It's Fast
Like the other web services, the HTML format is just a raw C# IHttpHandler using 
[.NET's fastest JSON Serializer](http://www.servicestack.net/mythz_blog/?p=344) 
to serialize the response DTO to a JSON string which is embedded inside a **static HTML string template**. 
No other .aspx page or MVC web framework is used to get in the way to slow things down.
High performance JavaScript techniques are also used to start generating the report at the earliest possible time.

### Well supported in all modern browsers
It's tested and works equally well on the latest versions of Chrome, Firefox, Safari, Opera and IE9.
[v1.83](https://github.com/ServiceStack/ServiceStack/downloads) Now works in IE8 but needs internet connection to download json2.js. (not tested in <=IE7)

### It Validates (as reported by validator.w3.org)
This shouldn't be important but as the technique of using JavaScript to generate the entire report is likely to ire the semantic HTML police, I thought I'd make this point. Personally I believe this style of report is more useful since it caters for both human and scripted clients.

# How it works - 'view-source' reveals all :)

This is a new type of HTML5 report that breaks the normal conventional techniques of generating a HTML page.
Instead of using a server programming and template language to generate the HTML on the server, the data is simply embedded as JSON, unaltered inside the tag:

    <script id="dto" type="text/json">{jsondto}</script>

Because of the browser behavior of the script tag, you can embed any markup or javascript unescaped.
Unless it has none or a 'javascript' type attribute, the browser doesn't execute it letting you to access the contents with:

    document.getElementById('dto').innerHTML
    
From there, javascript invoked just before the closing body tag (i.e. the fastest time to run DOM-altering javascript) which takes the data, 
builds up a html string and injects the generated markup in the contents of the page.

After the report is rendered, and because JavaScript can :) UX-friendly behaviors are applied to the document allowing the user to sort the table data on each column as well as providing an easy way to take a copy of the JSON data source.

For what it does, the JavaScript is very terse considering no JavaScript framework was used. In most cases the JSON payload is a lot larger than the entire JavaScript used to render the report :)

### Advantages of a strong-typed, code-first web service framework

Although hard to believe, most of the above web service examples were developed before ServiceStack's CSV and HTML format existed.
No code-changes were required in order to take advantage of the new formats, they were automatically available after replacing the ServiceStack.dlls with the latest version (v1.81+)

Being able to generically provide new features like this shows the advantage of ServiceStack's strong-typed, code-first approach to developing web services that lets you focus on your app-specific logic as you only need to return C#/.NET objects or throw C#/.NET exceptions which gets automatically handled, and hosted on a number of different endpoints in a variety of different formats.
 
Out of the box REST, RPC and SOAP endpoints types are automatically provided, in JSON, XML, CSV, JSV and now the new HTML report format above.

Follow [@demisbellot](http://twitter.com/demisbellot) and [@ServiceStack](http://twitter.com/ServiceStack) for twitter updates



[<Wiki Home](~/framework/home)