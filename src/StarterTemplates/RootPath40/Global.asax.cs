using System;
using StarterTemplates.Common;

namespace RootPath40
{
	public class Global : System.Web.HttpApplication
	{
		void Application_Start(object sender, EventArgs e)
		{
			new StarterTemplateAppHost().Init();
		}
	}
}
