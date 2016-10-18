# Base Starter Templates for different ServiceStack Hosts

These starter templates show the default configuration required to run ServiceStack under a number of different hosts:

  * RootPath45 - Host at '/' on .NET 4.5
  * RootPath40 - Host at '/' on .NET 4.0
  * CustomPath45 - Host at '/api' on .NET 4.5
  * CustomPath40 - Host at '/api' on .NET 4.0
  * ConsoleAppHost - Host as a stand-alone Console Application using HttpListener

Run run the script below to start the hosts above on VS.NET WebDev.WebServer.EXE at ports 5001-5004:
start_vs2010_webserver.bat

When embedding static files in a Console or Windows Service host, remember to set the Build Action = "Content" and Copy to Output Directory settings.

