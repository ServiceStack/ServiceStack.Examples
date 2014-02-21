using System;
using Funq;
using NUnit.Framework;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.Examples.ServiceInterface;
using ServiceStack.Examples.ServiceInterface.Support;
using ServiceStack.Logging;
using ServiceStack.OrmLite;

namespace ServiceStack.Examples.Tests.Integration
{
    public class IntegrationTestAppHost
        : AppHostHttpListenerBase
    {
		private static ILog log;

        public IntegrationTestAppHost()
			: base("ServiceStack Examples", typeof(MovieRestService).Assembly)
		{
			LogManager.LogFactory = new DebugLogFactory();
			log = LogManager.GetLogger(GetType());
			Instance = null;
		}	

		public override void Configure(Container container)
		{
            container.Register<IAppSettings>(new AppSettings());

            container.Register(c => new ExampleConfig(c.Resolve<IAppSettings>()));
			var appConfig = container.Resolve<ExampleConfig>();

			container.Register<IDbConnectionFactory>(c =>
				 new OrmLiteConnectionFactory(
					":memory:",			//Use an in-memory database instead
					SqliteDialect.Provider));

			ConfigureDatabase.Init(container.Resolve<IDbConnectionFactory>());
		}
    }

	public class IntegrationTestBase
	{
        private const string BaseUrl = "http://127.0.0.1:8080/";

        private readonly ServiceStackHost appHost;

	    public IntegrationTestBase()
	    {
            appHost = new IntegrationTestAppHost()
                .Init()
                .Start(BaseUrl);
	    }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            appHost.Dispose();
        }

	    public void SendToEachEndpoint<TRes>(object request, Action<TRes> validate)
		{
			SendToEachEndpoint(request, null, validate);
		}

		/// <summary>
		/// Run the request against each Endpoint
		/// </summary>
		/// <typeparam name="TRes"></typeparam>
		/// <param name="request"></param>
		/// <param name="validate"></param>
		/// <param name="httpMethod"></param>
		public void SendToEachEndpoint<TRes>(object request, string httpMethod, Action<TRes> validate)
		{
			using (var xmlClient = new XmlServiceClient(BaseUrl))
			using (var jsonClient = new JsonServiceClient(BaseUrl))
			using (var jsvClient = new JsvServiceClient(BaseUrl))
			{
				xmlClient.HttpMethod = httpMethod;
				jsonClient.HttpMethod = httpMethod;
				jsvClient.HttpMethod = httpMethod;

				var xmlResponse = xmlClient.Send<TRes>(request);
				if (validate != null) validate(xmlResponse);

				var jsonResponse = jsonClient.Send<TRes>(request);
				if (validate != null) validate(jsonResponse);

				var jsvResponse = jsvClient.Send<TRes>(request);
				if (validate != null) validate(jsvResponse);
			}
		}

	}
}
