<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ServiceStack.Hello._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ServiceStack.Hello :: The smallest ServiceStack Web Service</title>
    
    <style type="text/css">
    BODY 
    {
        background: #fff url(img/bg-body-1024.png) repeat-y top left;
        font: normal normal normal 13.34px/normal helvetica, arial, freesans, clean, sans-serif;
        padding: 0 5px 20px 10px;
        width: 940px;
    }
        
    code {
        background-color: ghostWhite !important;
        border: 1px solid #DEDEDE !important;
        color: #444 !important;
        font-size: 12px !important;
        line-height: 1.5em !important;
        margin: 1em 0px !important;
        overflow: auto !important;
        padding: 0.5em !important;
        
        display: block;
        font-family: monospace;
        margin: 1em 0px;
        white-space: pre;        
    }    
    
    H1 
    {
        border-top: 4px solid #AAA;
        font-size: 170% !important;
        margin-top: 1.5em !important;
        padding-top: 0.5em !important;
    }    
    H1.title 
    {
    	border: none;   
    }
    
    H2 
    {
        border-top: 4px solid #E0E0E0 !important;
        font-size: 150% !important;
        margin-top: 1.5em !important;
        padding-top: 0.5em !important;
    }
    
    .lnk-src 
    {
    	float: right;
    }
    
    </style>
</head>
<body>
    <a class="lnk-src" href="https://github.com/mythz/ServiceStack"><img src="img/btn-github.png" width="186" height="84" /></a>

    <h1 class="title">Creating a Hello World Service with ServiceStack</h1>
    
    <p>
        This tutorial will go through and build and run a simple Web Service from scratch using
        <a href="http://www.servicestack.net">Service Stack</a>.
    </p>
    

    <h2>1. Creating your application</h2>
    <p>
        Create a new ASP.NET Web Application by going into Visual Studio 
        and selecting <strong>File -&gt; New -&gt; Project</strong> on the File menu.
    </p>
    <p><img src="img/NewProject.png" alt="New ASP.NET Project" width="776" height="552" /></p>            
    
    <h2>2. Configuring Service Stack to run in your application</h2>
    <p>
        Add a reference to the <a href="https://github.com/mythz/ServiceStack/downloads">latest ServiceStack.dll's</a> to your project:
    </p>    
    
    <p>
        Register the ServiceStack handler with in your ASP.NET application 
        by adding either of the following to your <strong>Web.config</strong>:
    </p>

<h4>ServiceStack Handler mapping for IIS 6.0, MONO or VS.NET dev web server</h4>
<code class="web-config">&lt;system.web&gt;
  &lt;httpHandlers&gt;
    &lt;add path="ServiceStack*,ServiceStack*/*,ServiceStack*/*/*,ServiceStack*/*/*/*" type="ServiceStack.WebHost.Endpoints.ServiceStackHttpHandlerFactory, ServiceStack" verb="*"/&gt;
  &lt;/httpHandlers&gt;
&lt;/system.web&gt;
</code>      
            
<h4>ServiceStack Handler mapping for IIS 7+</h4>            
<code class="web-config">&lt;system.webServer&gt;
  &lt;handlers&gt;
    &lt;add name="ServiceStack.Factory" path="ServiceStack" type="ServiceStack.WebHost.Endpoints.ServiceStackHttpHandlerFactory, ServiceStack" verb="*" preCondition="integratedMode" resourceType="Unspecified" allowPathInfo="true" /&gt;
  &lt;/handlers&gt;
&lt;/system.webServer&gt;
</code>    

    <h2>3. Creating your first Web Service</h2>

    Add a Global.asax Start Up Script. <br />            
    For simplicity we will create the Web Service and the start up script in the same Global.asax.cs:          
    
    <h4>1. Create the name of your Web Service (i.e. the Request DTO)</h4>
<code class="csharp">[DataContract]
[RestService("/hello/{Name}")] //Optional: Define an alternate REST-ful url for this service
public class Hello
{
    [DataMember]
    public string Name { get; set; }
}                
</code>

    <h4>2. Define what your Web Service will return (i.e. Response DTO)</h4>
<code class="csharp">[DataContract]
public class HelloResponse
{
    [DataMember]
    public string Result { get; set; }
}
</code>

    <h4>3. Create your Web Service implementation</h4>
<code class="csharp">public class HelloService : IService&lt;Hello&gt;
{
    public object Execute(Hello request)
    {
        return new HelloResponse { Result = "Hello, " + request.Name };
    }
}
</code>

            
    <h3>Registering your web services and starting your application</h3>
    
    The final step is to Configure setup to tell Service Stack where to find your web services:

    <h4>Web Service AppHost Singleton</h4> 
<code class="csharp">public class Global : System.Web.HttpApplication
{
    public class HelloAppHost : AppHostBase
    {
        //Tell Service Stack the name of your application and where to find your web services
        public HelloAppHost() 
	        : base("Hello Web Services", typeof(HelloService).Assembly) { }
		
        public override void Configure(Container container) { }
    }

    //Initialize your application singleton
    protected void Application_Start(object sender, EventArgs e)
    {
        var appHost = new HelloAppHost();
        appHost.Init();
    }
}
</code>
    
    <p>
        Done! You now have a working application :)
    </p>
    


    <h1>Viewing your Web Services</h1>
    <p>
        Now that you have a working Web Service lets see what ServiceStack does for you out of the box:
    </p>

    <h4>The Web Service Index Metadata page</h4>
    <p>
        If everything is configured correctly you can go to the 
        <strong><a href="servicestack/metadata">/servicestack/metadata</a></strong>
        to see a list of your web services and the various end points its available on.
    </p>
    
    <p><img src="img/MetadataIndex.png" alt="Service Stack Metadata page" width="747" height="647" /></p>
    
    <h2>Different ways of accessing your Web Services</h2>
    
    <p>
        One of the advantages of ServiceStack is that you only have to write your web service
        once and it takes care of
    </p>
    
    <h3>Calling your web services with a single URL</h3>
    <p>By default ServiceStack lets you call following end points using just a url:</p>
    <dl>
        <dt>XML</dt>
        <dd><a href="servicestack/xml/syncreply/Hello?Name=World">/servicestack/xml/syncreply/Hello?Name=World</a></dd>
        <dt>JSON</dt>
        <dd><a href="servicestack/json/syncreply/Hello?Name=World">/servicestack/json/syncreply/Hello?Name=World</a></dd>
        <dt>JSV</dt>
        <dd><a href="servicestack/jsv/syncreply/Hello?Name=World">/servicestack/jsv/syncreply/Hello?Name=World</a></dd>
    </dl>
    
    <h2>Calling your web services with a HTTP POST</h2>
    <p>
        In addition to calling the above Web Services using just a HTTP GET, 
        they can also call them via a HTTP POST at the following locations:
    </p>
    <dl>
        <dt>XML</dt>
        <dd><a href="servicestack/xml/metadata?op=Hello">/http://localhost/ServiceStack.Hello/servicestack/xml/Hello</a></dd>
        <dt>JSON</dt>
        <dd><a href="servicestack/json/metadata?op=Hello">/http://localhost/ServiceStack.Hello/servicestack/json/Hello</a></dd>
        <dt>JSV</dt>
        <dd><a href="servicestack/jsv/metadata?op=Hello">/http://localhost/ServiceStack.Hello/servicestack/jsv/Hello</a></dd>
    </dl>    
    <p>
        The above Web Services also accept HTTP POST of Content-Type: of
        <strong>application/x-www-form-urlencoded</strong> or <strong>multipart/form-data</strong>        
    </p>
    
    <dl>
    </dl>
    
</body>
</html>
