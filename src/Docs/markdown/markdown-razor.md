See other [Markdown Features](markdown-features) in ServiceStack and how this Docs website 
makes use of them in [About Docs](about).

# Introduction

Markdown Razor is the first HTML and Text (i.e. Markdown) view engine built into ServiceStack.
The pages are simply plain-text Markdown surrounded by MVC Razor-like syntax which provides its 
dynamic functionality.

## Extensible with custom base classes and Helpers
Markdown Razor is extensible in much the same way as MVC Razor is with the ability to define and 
use your own **custom base class**, **Helpers** and **HtmlHelper** extension methods. 
This allows you to call directly call util methods on your base class or helpers directly from 
your templates.

You can define a base class for all your markdown pages by implementing **MarkdownViewBase** and
register it in your AppHost with:

    SetConfig(new EndpointHostConfig {
        WebHostUrl = "http://servicestack.net/docs",   //Replace prefix with the Url supplied
        MarkdownBaseType = typeof(CustomMarkdownPage), //Set base class for all Markdown pages
        MarkdownGlobalHelpers = new Dictionary<string, Type> { 
            {"Ext", typeof(CustomStaticHelpers)}       //Define global Helpers e.g. at Ext.
        }
    });

If a **WebHostUrl** is specified, it replaces all **&#126/** in all static website and Markdown pages with it.
Whilst **MarkdownGlobalHelpers** allows you to dynamically define helper methods available to all
your pages. This has the same effect of declaring it in your base class e.g:

    public class CustomMarkdownPage : MarkdownViewBase {
	    public CustomStaticHelpers Ext = new CustomStaticHelpers();
	}

Which you can access in your pages via **&#64;Ext.MyHelper(Model)**. Declaring instance methods on
your custom base class allows you to access them without any prefix.


## MarkdownViewBase base class
By default the **MarkdownViewBase** class provides the following properties and hooks:

    public class MarkdownViewBase {
		
		public IAppHost AppHost; //Access Config, resolve dependencies, etc.
		public MarkdownPage MarkdownPage; //This precompiled Markdown page with Metadata
		public HtmlHelper Html; //ASP.NET MVC's HtmlHelper
		public bool RenderHtml; //Flag to on whether you should you generate HTML or Markdown

		/*
		  All variables passed to and created by your page. 
		  The Response DTO is stored and accessible via the 'Model' variable.

		  All variables and outputs created are stored in ScopeArgs which is what's available
		  to your website template. The Generated page is stored in the 'Body' variable.
        */
		public Dictionary<string,object> ScopeArgs;

		//Called before page is executed
        public virtual void InitHelpers(){} 

        //Called after page is executed before it's merged with the website template if any
		public virtual void OnLoad(){}      
    }

See this websites **CustomMarkdownPage.cs** base class for an example on how to effectively use
the base class to Resolve dependencies, inspect generated variables, generate **PagesMenu** and 
other dynamic variables for output in the static website template.

# Compared with ASP.NET MVC Razor Syntax

For the best way to illustrate the similarities with ASP.NET MVC Razor syntax I will show examples
of the Razor examples in [@ScottGu's](http://twitter.com/scottgu) introductory 
[Introducing "Razor" - a new view engine for ASP.NET](http://weblogs.asp.net/scottgu/archive/2010/07/02/introducing-razor.aspx)

Note: more context and the output for each snippet and example displayed is contained in the 
[Introductory Example](https://github.com/ServiceStack/ServiceStack/blob/master/tests/ServiceStack.ServiceHost.Tests/Formats/IntroductionExampleTests.cs)
and 
[Introductory Layout](https://github.com/ServiceStack/ServiceStack/blob/master/tests/ServiceStack.ServiceHost.Tests/Formats/IntroductionLayoutTests.cs)
Unit tests. For reference most features of Mardown Razor view engine are captured in the
[Core Template Unit Tests](https://github.com/ServiceStack/ServiceStack/blob/master/tests/ServiceStack.ServiceHost.Tests/Formats/TemplateTests.cs)

### Hello World Sample with Razor


The following basic page: 

![Hello World Output](http://weblogs.asp.net/blogs/scottgu/image_thumb_0E9E3527.png)


Can be generated in MVC Razor with:

![Hello World MVC Razor](http://weblogs.asp.net/blogs/scottgu/image_1B981538.png)


And Markdown Razor with:

	# Razor Example

	###  Hello @@name, the year is @@DateTime.Now.Year

	Checkout [this product](/Product/Details/@@productId)

### Loops and Nested HTML Sample

The simple loop example:

![Simple Loop Output](http://weblogs.asp.net/blogs/scottgu/image_thumb_155D5078.png)


With MVC Razor:

![Simple Loop MVC Razor](http://weblogs.asp.net/blogs/scottgu/image_thumb_39360205.png)


With Markdown Razor:

	@@foreach (var p in products) {
	  - @@p.Name: (@@p.Price)
	}

### Parens-Free
At this point I think it would be a good to introduce some niceties in Markdown Razor of its own. 
Borrowing a page out of [@BrendanEich](http://twitter.com/BrendanEich) proposal for 
[CoffeeScript's](http://jashkenas.github.com/coffee-script/) inspired 
[Parens free syntax](http://brendaneich.com/2010/11/paren-free/) for JS.Next - you can simply 
remove the parens for all block statements e.g:

	@@foreach var p in products {
	  - @@p.Name: (@@p.Price)
	}

Produces the same output, and going one step further you can remove the redundant **var** as well:

	@@foreach p in products {
	  - @@p.Name: (@@p.Price)
	}

Which makes the Markdown Razor's version a bit more wrist-friendly then its MVCs cousin :)

### If-Blocks and Multi-line Statements

#### If statements in MVC Razor:
![If Statements](http://weblogs.asp.net/blogs/scottgu/image_thumb_4C76B8A4.png)

#### If statements in Markdown Razor:

	@@if (products.Count == 0) {
	Sorry - no products in this category
	} else {
	We have products for you!
	}

#### Multi-line and Multi-token statements
![MVC Razor Multi-line statements](http://weblogs.asp.net/blogs/scottgu/image_thumb_4B321FC5.png)

Markdown Razor doesn't support multi-line or multi-token statements, instead you are directed to
take advantage for variable syntax declarations, e.g:

#### Markdown replacement for Multi-line Statements

	@@var number = 1
	@@var message = ""Number is "" + number

	Your Message: @@message


### Integrating Content and Code

Does it break with email addresses and other usages of @ in HTML?

#### With MVC Razor
![MVC Razor Content and Code](http://weblogs.asp.net/blogs/scottgu/image_22B33DB1.png)

#### With Markdown Razor

	Send mail to scottgu@microsoft.com telling him the time: @@DateTime.Now.

Both View engines generate the expected output, e.g:

![MVC Razor Content and Code Output](http://weblogs.asp.net/blogs/scottgu/image_thumb_20963EE8.png)


### Identifying Nested Content

#### With MVC Razor
![MVC Razor Identifying Nested Content](http://weblogs.asp.net/blogs/scottgu/image_thumb_4A2A0A1B.png)

#### With Markdown Razor

	@@if (DateTime.Now.Year == 2011) {
	If the year is 2011 then print this 
	multi-line text block and 
	the date: @@DateTime.Now
	}

Markdown Razor doesn't need to do anything special with text blocks since all it does is look 
for the ending brace '}'. This means if you want to output a brace '{' literal then you have to 
double escape it, i.e. '{{'.

### HTML Encoding
Markdown Razor follows MVC Markdown behaviour where by default content emitted using a @@ block 
is automatically HTML encoded to better protect against XSS attack scenarios. 

If you want to avoid HTML Encoding you have the same options as MVC Razor where you can wrap your
result in **@@Html.Raw(htmlString)** or if you're using an Extension method simply return a 
**MvcHtmlString** instead of a normal string.


# Layout/MasterPage Scenarios - The Basics

Markdown Razor actually deviates a bit from MVC Razor's handling of master layout pages and 
website templates (we believe for the better :). 

### Simple Layout Example

#### MVC Razor's example of a simple website template
![MVC Razor simple website template](http://weblogs.asp.net/blogs/scottgu/image_thumb_55CF2B80.png)

Rather then using a magic method like `@@RenderBody()` we treat the output Body of View as just 
another variable storing the output a in a variable called **'Body'**. This way we use the
same mechanism to embed the body like any other variable i.e. following the place holder convention
of **&lt;--@@VarName--&gt;** so to embed the View page output in the above master template you 
would do:

	<!DOCTYPE html>
	<html>
		<head>
			<title>Simple Site</title>
		</head>
		<body>
    
			<div id=""header"">
				<a href=""/"">Home</a>
				<a href=""/About"">About</a>
			</div>
        
			<div id=""body"">
				<!--@@Body-->
			</div>
    
		</body>
	</html>

By default we use convention to associate the appropriate website template with the selected view 
page where it uses the nearest **default.shtml** static template it finds, looking first in the
current directory than up parent directories. 

Your View page names must be unique but can live anywhere in your **/View** directory so you are 
free to structure your website templates and view pages accordingly. If for whatever you need more
granularity in selecting website templates than we provide similar options to MVC for selecting a
custom template:

#### Select Custom Template with MVC Razor
![MVC Razor Custom Layout Page](http://weblogs.asp.net/blogs/scottgu/image_thumb_3B228F67.png)

#### With Markdown Razor

	@@LayoutPage ~/websiteTemplate

	# About this Site

	This is some content that will make up the ""about"" 
	page of our web-site. We'll use this in conjunction
	with a layout template. The content you are seeing here
	comes from ^^^websiteTemplate.

	And obviously I can have code in here too. Here is the
	current date/year: @@DateTime.Now.Year

Note: In addition to **@@LayoutPage** we also support the more appropriate alias of **@@template**.

## Layout/MasterPage Scenarios - Adding Section Overrides

MVC Razor allows you to define **sections** in your view pages which you can embed in your 
Master Template:

#### With MVC Razor:
![MVC Razor Sections in Views](http://weblogs.asp.net/blogs/scottgu/image_thumb_448B2810.png)

And you use in your website template like so:
![MVC Razor use Sections](http://weblogs.asp.net/blogs/scottgu/image_thumb_6D0A0A24.png)

#### With Markdown Razor:
Markdown Razor supports the same **@@section** construct but allows you to embed it in your template
via the standard variable substitution convention, e.g:

	@@LayoutPage ~/websiteTemplate

	# About this Site

	This is some content that will make up the ""about"" 
	page of our web-site. We'll use this in conjunction
	with a layout template. The content you are seeing here
	comes from ^^^websiteTemplate.

	And obviously I can have code in here too. Here is the
	current date/year: @@DateTime.Now.Year

	@@section Menu {
	  - About Item 1
	  - About Item 2
	}

	@@section Footer {
	This is my custom footer for Home
	}

And these sections and body can be used in the website template like:

	<!DOCTYPE html>
	<html>
		<head>
			<title>Simple Site</title>
		</head>
		<body>
    
			<div id="header">
				<a href="/">Home</a>
				<a href="/About">About</a>
			</div>
        
			<div id="left-menu">
				<!--@@Menu-->
			</div>
        
			<div id="body">
				<!--@@Body-->
			</div>
        
			<div id="footer">
				<!--@@Footer-->
			</div>
    
		</body>
	</html>

## Encapsulation and Re-Use with HTML Helpers

In order to encapsulate and better be able to re-use HTML Helper utils MVC Razor includes a few 
different ways to componentize and re-use code with HTMLHelper extension methods and declarative helpers. 

### Code Based HTML Helpers

#### HtmlHelper extension methods with MVC Razor:
![MVC Razor HtmlHelper extension methods](http://weblogs.asp.net/blogs/scottgu/image_thumb_150C9377.png)

Since we've ported MVC's HtmlHelper and its **Label**, **TextBox** extensions we can do something
similar although to make this work we need to inherit from the **MarkdownViewBase&lt;TModel&gt;**
generic base class so we know what Model to provide the strong-typed extensions for. You can do this
using the **@@model** directive specifying the full type name:

	@@model ServiceStack.ServiceHost.Tests.Formats.Product
	<fieldset>
		<legend>Edit Product</legend>
    
		<div>
			@@Html.LabelFor(m => m.ProductID)
		</div>
		<div>
			@@Html.TextBoxFor(m => m.ProductID)
		</div>
	</fieldset>

Whilst we ported most of MVC HtmlHelper extension methods as-is, we did rip out all the validation
logic which appeared to be unnecessarily complex and too coupled with MVC's code-base.

Note: Just as it is in MVC the **@@model** directive is a shorthand (which Markdown Razor also supports) for: 

**@@inherits Namespace.BaseType&lt;Namespace.ModelType&gt;** 

Whilst we don't support MVC Razors quasi C# quasi-html approach of defining declarative helpers,
we do allow you to on a per instance basis (or globally) import helpers in custom Fields using the 
**@@helper** syntax:

	@@helper Prod: MyHelpers.ExternalProductHelper

	<fieldset>
		<legend>All Products</legend>
		@@Prod.ProductTable(Model)
	</fieldset

You can register Global helpers and a custom base class using the **MarkdownGlobalHelpers** and
**MarkdownBaseType** AppHost Config options as shown at the top of this article.

# Summary

Well that's it for the comparison between MVC Razor and Markdown Razor as you can see the knowledge
is quite transferable with a lot of cases the syntax is exactly the same.

As good as MVC Razor is with its wrist-friendly and expressive syntax, we believe Razor Markdown is
even more so where thanks to Markdown you can even dispense with most of HTML's boiler plage 
angle brackets. We think it makes an ideal solution for content heavy websites like this one.

Unlike ASP.NET's MVC Razor, Markdown Razor like all of ServiceStack is **completely Open Source** and 
as such we welcome the contribution from the community via new features, Unit and 
regression tests, etc.

As this is the first beta release of **Markdown Razor** please let us know of any issues on our 
group at: [http://groups.google.com/group/servicestack](http://groups.google.com/group/servicestack)

Peace and [Markdown](http://daringfireball.net/projects/markdown/) FTW!
