@ECHO OFF

SET HOST_ROOT="C:\src\ServiceStack.Examples\src\StarterTemplates"

SET WEB_SERVER="C:\Program Files\Common Files\microsoft shared\DevServer\10.0\WebDev.WebServer20.EXE"

%WEB_SERVER% /port:5001 /path:"%HOST_ROOT%\CustomPath35" /vpath:"/"
%WEB_SERVER% /port:5003 /path:"%HOST_ROOT%\RootPath35" /vpath:"/"

SET WEB_SERVER="C:\Program Files\Common Files\microsoft shared\DevServer\10.0\WebDev.WebServer40.EXE"

%WEB_SERVER% /port:5002 /path:"%HOST_ROOT%\CustomPath40" /vpath:"/"
%WEB_SERVER% /port:5004 /path:"%HOST_ROOT%\RootPath40" /vpath:"/"


REM HttpListener
%HOST_ROOT%\ConsoleAppHost\bin\Debug\ConsoleAppHost.exe