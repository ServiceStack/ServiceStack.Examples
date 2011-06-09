[Join the new google group](http://groups.google.com/group/servicestack) or
follow [@demisbellot](http://twitter.com/demisbellot) and [@ServiceStack](http://twitter.com/servicestack)
for twitter updates.

#ServiceStack Redis Web Services including the Redis Admin UI
Included is a ServiceStack web service layer which provide JSON, XML, JSV and SOAP 1.1/1.2 for all of Redis operations.
Just like the RedisAdminUI this allows you to fully manage your redis-server instance using javascript from a browser.

##Live Demo
A live demo of the RedisAdminUI can be found here [http://servicestack.net/RedisAdminUI/AjaxClient/](http://servicestack.net/RedisAdminUI/AjaxClient/)

View the demos live list of the [available web services](http://www.servicestack.net/RedisAdminUI/servicestack/metadata).


#Download
[ServiceStack.RedisWebServices/downloads](https://github.com/ServiceStack/ServiceStack.RedisWebServices/downloads)

##Troubleshooting
Note: if running via XSP you will want to change the 'DefaultRedirectPath' to:

    <add key="DefaultRedirectPath" value="AjaxClient/default.htm"/>
