
**ServiceStack Docs** is a collection of all documentation scattered amongst all of ServiceStack's Open source
projects hosted together in a single website. It's also the first website to showcase ServiceStack's
new support of the Markdown format where the website's content is composed entirely of static Markdown 
pages collected from the project's README.md and wiki Markdown pages spread in GitHub.

# ServiceStack Docs Features

### Static Templates
All content pages simply consist of a static Markdown page merged together with a static 
**default.shtml** page where its variable place holders are replaced with evaluated Markdown content.

### Partial Content
On the header of each content page there are links to view the raw [HTML content only](?format=html.bare) 
or [Markdown only](?format=text.bare) of the current page outside of the website template.

### Non-invasive Ajax-enhanced web experience
To show how to take advantage of the partial-content support in ServiceStack, browsers 
that support the **history.pushState** API have enhanced behavior applied to them where
links to other parts of the website are done via non-obtrusive partial page loads. 

This improves browsing experience performance since the browser only has to load the content html
and not the entire web page. By not reloading the page we are able to control the pages transitions
where in our case we sprinkle a little jQuery to apply a simple slide and fade effect between pages.

### Dynamic content with Markdown Razor
Although most of this website is static it contains a couple of dynamic pages that makes use of the new
Markdown Razor View engine template in ServiceStack. The [Search](~/search/Redis) 
and [Category](~/category/Redis%20Client) web services both have
view templates which when defined take over the **html format** for that REST service. Unlike other
web frameworks these dynamic pages are **first-class web services** where you can optionally use 
REST clients to consume these services in JSON, XML, ... formats e.g. here are the above pages in JSON: 

  - [/search/Redis?format=json](~/search/Redis?format=json)
  - [/category/Redis Client?format=json](~/category/Redis%20Client?format=json)

### Extensible output
Each markdown page is configured to inherit from the **CustomMarkdownPage.cs** base class which is used to 
generate the **dynamic sidebar** based on the context of the currently viewed page. Other metadata like 
the **Title**, **Category** and document **Date** are also added and displayed in the static website template.

### Find out more about Markdown support

  - [Markdown Features](markdown-features) - Benefits and Markdown features in ServiceStack
  - [Markdown Razor](markdown-razor) - Introduction to the new Markdown View Engine and its Razor syntax in ServiceStack