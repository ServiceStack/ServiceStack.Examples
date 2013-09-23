using System;
using ServiceStack.MovieRest.App_Start;

namespace ServiceStack.MovieRest
{
	using System.Web;

	public class Global : HttpApplication
	{
        protected void Application_Start(object sender, EventArgs e)
        {
            //Initialize your application
            (new AppHost()).Init();
        }
	}
}