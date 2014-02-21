using System;
using System.Configuration;
using System.IO;
using ServiceStack.Configuration;

namespace ServiceStack.Examples.Host.Web
{
	public class Global : System.Web.HttpApplication
	{

		protected void Application_Start(object sender, EventArgs e)
		{
            if (File.Exists(@"C:\src\appsettings.license.txt"))
                Licensing.RegisterLicenseFromFile(@"C:\src\appsettings.license.txt");
            else if (string.IsNullOrEmpty(ConfigUtils.GetNullableAppSetting("servicestack:license")))
                throw new ConfigurationErrorsException("A valid license key is required for this demo");

			var appHost = new AppHost();
			appHost.Init();
		}

		protected void Session_Start(object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{

		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{
		}
	}
}