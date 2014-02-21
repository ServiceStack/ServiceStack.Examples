using System;
using Funq;
using NUnit.Framework;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.Examples.ServiceInterface;
using ServiceStack.Examples.ServiceInterface.Support;
using ServiceStack.Host;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using ServiceStack.Testing;

namespace ServiceStack.Examples.Tests
{
	[TestFixture]
	public class TestHostBase
	{
		protected const string InMemoryDb = ":memory:";
		private static ILog log;

	    public readonly ServiceStackHost appHost;

		public TestHostBase()
		{
            LogManager.LogFactory = new ConsoleLogFactory();
            log = LogManager.GetLogger(GetType());

            appHost = new BasicAppHost(typeof(GetFactorialService).Assembly) {
                ConfigureContainer = Configure
            }.Init();
		}

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            appHost.Dispose();
        }

		protected IDbConnectionFactory ConnectionFactory
		{
			get
			{
                return appHost.Container.Resolve<IDbConnectionFactory>();
			}
		}		

		public void Configure(Container container)
		{
			container.Register<IAppSettings>(c => new AppSettings());

			container.Register<IDbConnectionFactory>(c =>
				new OrmLiteConnectionFactory(
					InMemoryDb,
					SqliteDialect.Provider));

			ConfigureDatabase.Init(container.Resolve<IDbConnectionFactory>());

			log.InfoFormat("TestAppHost Created: " + DateTime.Now);
		}
		
		/// <summary>
		/// Process a webservice in-memory
		/// </summary>
		/// <typeparam name="TResponse"></typeparam>
		/// <param name="request"></param>
		/// <returns></returns>
		public TResponse Send<TResponse>(object request)
		{
            return Send<TResponse>(request, RequestAttributes.None);
		}

        public TResponse Send<TResponse>(object request, RequestAttributes endpointAttrs)
		{
            return (TResponse)appHost.ServiceController.Execute(request,
				new BasicRequest(request, endpointAttrs));
		}
	}
}