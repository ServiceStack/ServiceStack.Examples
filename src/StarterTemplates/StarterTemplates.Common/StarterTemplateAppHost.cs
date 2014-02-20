using ServiceStack;
using ServiceStack.Configuration;

namespace StarterTemplates.Common
{

	//ASP.NET Hosts
	public class StarterTemplateAppHost
		: AppHostBase
	{
        static readonly IAppSettings AppSettings = new AppSettings();

		public StarterTemplateAppHost()
			: base(AppSettings.GetString("ServiceName") ?? "StarterTemplate ASP.NET Host", typeof(HelloService).Assembly) { }

		public override void Configure(Funq.Container container)
		{
			container.Register(new TodoRepository());
		}
	}

	//HttpListener Hosts
	public class StarterTemplateAppListenerHost
		: AppHostHttpListenerBase
	{
        static readonly IAppSettings AppSettings = new AppSettings();

		public StarterTemplateAppListenerHost()
			: base(AppSettings.GetString("ServiceName") ?? "StarterTemplate HttpListener", typeof(HelloService).Assembly) { }

		public override void Configure(Funq.Container container)
		{
			container.Register(new TodoRepository());
		}
	}
}
