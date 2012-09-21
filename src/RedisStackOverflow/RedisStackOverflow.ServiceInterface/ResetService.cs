using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Common.Extensions;
using ServiceStack.Redis;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace RedisStackOverflow.ServiceInterface
{
    /// <summary>
    /// Define your ServiceStack web service request (i.e. the Request DTO).
    /// </summary> 
    [Route("/reset")]
    public class Reset { }

    /// <summary>
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    public class ResetResponse : IHasResponseStatus
    {
        public ResetResponse()
        {
            //Comment this out if you wish to receive the response status.
            this.ResponseStatus = new ResponseStatus();
        }

        /// <summary>
        /// Gets or sets the ResponseStatus. The built-in Ioc used with ServiceStack autowires this property with service exceptions.
        /// </summary>
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    /// Create your ServiceStack rest-ful web service implementation. 
    /// </summary>
    public class ResetService : RestServiceBase<Reset>
    {
        /// <summary>
        /// Gets or sets the Redis Manager. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
        public IRedisClientsManager RedisManager { get; set; }

        /// <summary>
        /// Gets or sets the repository. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>
        public IRepository Repository { get; set; }

        private static readonly Dictionary<string, string> JavascriptQuestions = new Dictionary<string, string> {
            {"a javascript string problem?","i have this script, but i the result.bgimage is not showing up, i think JS thinks its a string rather than variable!!! how can i solve this problem? thanks"},                                                                                   
            {"Control events of Object (HTML) by JavaScript","Is it possible to control an object that contains a swf that streams music, I want to be able to pause it by JS, but I don't know if that's possible or not."},                                                                                   
            {"Javascript function not working with an array","i have this function which loops through an array of check boxes checking if the boxes value is equal to something in a text box, i dont know whats wrong."},                                                                                   
            {"Prototypal programming in Python","Javascript uses a prototype-based model for its objects. Nevertheless, the language is very flexible, and it is easy to write in a few lines functions which replace other kind on constructs. For instance, one can make a class function, emulating the standard class behaviour, including inheritance or private members. Or one can mimc�ic functional tools by writing, for instance, a curry function which will take a function and some of its arguments and return the partially applied function."},
        };

        static readonly Dictionary<string, string> RestQuestions = new Dictionary<string, string> {
            {"How do I implement login in a RESTful web service?","I am building a web application with a services layer. The services layer is going to be built using a RESTful design. The thinking is that some time in the future we may build other applications (iPhone, Android, etc.) that use the same services layer as the web application. My question is this - how do I implement login? I think I am having trouble moving from a more traditional verb based design to a resource based design. If I was building this with SOAP I would probably have a method called Login. In REST I should have a resource."},                                                                                   
            {"Framework for providing API access to website?","We have a website that we want to provide web based API access to to other sites. It may end up being a REST based API, but I'm not sure yet. It needs to be accessible from a Drupal module, but we want to built the API to be scalable so that we can access the site's data and functionality from other environments such as joomla, wordpress, other non-php languages, etc. I am looking for a robust/stable PHP based framework that allows me to create such APIs - can folks suggest something that meets the criteria?"},                                                                                   
            {"Does HATEOAS imply query strings are not RESTful?","Does the HATEOAS (hypermedia as the engine of app state) recommendation imply that query strings are not RESTful?"},                                                                                   
            {"Drupal as backend for RESTful API?","Are there any good write-ups on creating RESTful APIs with Drupal? I see the services API, which I guess is how it's done. What I'm looking for, I suppose is a comparison of drupal vs. other frameworks for that particular purpose."},                                                                                   
        };

        public Question ToQuestion(string title, string body, List<string> tags)
        {
            return new Question
            {
                Title = title,
                Content = body,
                Tags = tags,
            };
        }

        public override object OnGet(Reset request)
        {
            //Uncomment if you want this feature
            //throw new NotSupportedException("Disabling for Demo site. Based on the XSS attacks I know it will only be a matter of time before someone pulls the trigger.");

            var questionsd = new Dictionary<string, string>(RestQuestions);
            JavascriptQuestions.ForEach(questionsd.Add);

            var questions = new List<Question>();
            questions.AddRange(RestQuestions.ConvertAll(kvp => ToQuestion(kvp.Key, kvp.Value, new List<string> { "rest", "http" })));
            questions.AddRange(JavascriptQuestions.ConvertAll(kvp => ToQuestion(kvp.Key, kvp.Value, new List<string> { "javascript", "jquery" })));


            RedisManager.Exec(r => r.FlushAll());

            var mythz = Repository.GetOrCreateUser(new User { DisplayName = "mythz" });
            questions.ForEach(q => q.UserId = mythz.Id);

            questions.ForEach(q => Repository.StoreQuestion(q));

            return new ResetResponse();
        }
    }
}