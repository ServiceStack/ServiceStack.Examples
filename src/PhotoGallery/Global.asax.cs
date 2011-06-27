using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using Funq;
using PhotoGallery.Logic;
using RazorEngine;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;

namespace PhotoGallery
{
	public class AppHost : AppHostBase
	{
		public AppHost()
			: base("Photo Gallery", typeof(HelloService).Assembly) { }

		public override void Configure(Container container)
		{
			SetConfig(new EndpointHostConfig {
				RazorBaseType = typeof(BasePage<>),
			});

			RazorFormat.Register(this);
		}
	}

	public class Global : System.Web.HttpApplication
	{
		void Application_Start(object sender, EventArgs e)
		{
			new AppHost().Init();
		}
	}
}
