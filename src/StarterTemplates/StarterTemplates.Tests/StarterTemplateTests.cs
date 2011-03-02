using System;
using System.IO;
using System.Net;
using NUnit.Framework;

namespace StarterTemplates.Tests
{
	public class StarterTemplateTestsBase
	{
		public static string BaseUrl = "http://localhost";

		private readonly string hostUrl;
		public StarterTemplateTestsBase(string hostUrl)
		{
			this.hostUrl = hostUrl;
		}

		public void TestUrl(string url)
		{
			try
			{
				var webReq = (HttpWebRequest)WebRequest.Create(url);
				var webRes = (HttpWebResponse)webReq.GetResponse();
				var statusCode = (int)webRes.StatusCode;				

				if (webRes.ContentLength == 0)
				{
					Assert.Fail("Server returned empty request for: " + url + ", Status: " + statusCode);
				}

				Console.WriteLine("'{0}' => {1} / {2} of {3} bytes", url, statusCode, webRes.ContentType, webRes.ContentLength);
			}
			catch (WebException webEx)
			{
				var webRes = (HttpWebResponse)webEx.Response;

				var errorResponse = ((HttpWebResponse)webEx.Response);

				var responseText = new StreamReader(errorResponse.GetResponseStream()).ReadToEnd();

				Console.WriteLine("'" + url + "' failed with status: " + ((int)webRes.StatusCode));
				Console.WriteLine(responseText);
				Assert.Fail(webEx.Message);
			}
		}

		[Test]
		public void Test_BaseUrl()
		{
			TestUrl(hostUrl);
		}

		[Test]
		public void Test_BaseUrl_with_slash()
		{
			TestUrl(hostUrl + "/");
		}

		[Test]
		public void Test_RequestInfo()
		{
			TestUrl(hostUrl + "/_requestinfo");
		}

		[Test]
		public void Test_Metadata()
		{
			TestUrl(hostUrl + "/metadata");
		}

		[Test]
		public void Test_Metadata_with_slash()
		{
			TestUrl(hostUrl + "/metadata/");
		}

		[Test]
		public void Test_Hello()
		{
			TestUrl(hostUrl + "/hello");
		}

		[Test]
		public void Test_Hello_with_slash()
		{
			TestUrl(hostUrl + "/hello/");
		}

		[Test]
		public void Test_Hello_world()
		{
			TestUrl(hostUrl + "/hello/world");
		}

		[Test]
		public void Test_Hello_world_with_1_arg()
		{
			TestUrl(hostUrl + "/hello/world/1");
		}

		[Test]
		public void Test_Hello_world_with_2_args()
		{
			TestUrl(hostUrl + "/hello/world/1/2");
		}

		[Test]
		public void Test_Hello_world_with_3_args()
		{
			TestUrl(hostUrl + "/hello/world/1/2/3");
		}

		[TestFixture]
		public class ConsoleAppHostTests : StarterTemplateTestsBase
		{
			public ConsoleAppHostTests() : base(BaseUrl + ":82") { }
		}


		[TestFixture]
		public class CustomPath35IisTests : StarterTemplateTestsBase
		{
			public CustomPath35IisTests() : base(BaseUrl + "/CustomPath35/api") { }
		}

		[TestFixture]
		public class CustomPath40IisTests : StarterTemplateTestsBase
		{
			public CustomPath40IisTests() : base(BaseUrl + "/CustomPath40/api") { }
		}

		[TestFixture]
		public class RootPath35IisTests : StarterTemplateTestsBase
		{
			public RootPath35IisTests() : base(BaseUrl + "/RootPath35") { }
		}

		[TestFixture]
		public class RootPath40IisTests : StarterTemplateTestsBase
		{
			public RootPath40IisTests() : base(BaseUrl + "/RootPath40") { }
		}


		[TestFixture]
		public class CustomPath35WebDevTests : StarterTemplateTestsBase
		{
			public CustomPath35WebDevTests() : base(BaseUrl + "/CustomPath35/api") { }
		}

		[TestFixture]
		public class CustomPath40WebDevTests : StarterTemplateTestsBase
		{
			public CustomPath40WebDevTests() : base(BaseUrl + "/CustomPath40/api") { }
		}

		[TestFixture]
		public class RootPath35WebDevTests : StarterTemplateTestsBase
		{
			public RootPath35WebDevTests() : base(BaseUrl + "/RootPath35") { }
		}

		[TestFixture]
		public class RootPath40WebDevTests : StarterTemplateTestsBase
		{
			public RootPath40WebDevTests() : base(BaseUrl + "/RootPath40") { }
		}


		[TestFixture]
		public class WinServiceAppHostTests : StarterTemplateTestsBase
		{
			public WinServiceAppHostTests() : base(BaseUrl + ":83") { }
		}
	}
}
