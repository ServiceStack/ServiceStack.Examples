using System;
using System.IO;
using System.Net;
using System.Threading;
using NUnit.Framework;
using RestFiles.ServiceModel.Operations;
using ServiceStack.Common.Web;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

/* For syntax highlighting and better readability of this file, view it on GitHub:
 * https://github.com/ServiceStack/ServiceStack.Examples/blob/master/src/RestFiles/RestFiles.Tests/AsyncRestClientTests.cs
 */

namespace RestFiles.Tests
{
	/// <summary>
	/// These test show how you can call ServiceStack REST web services asynchronously using an IRestClientAsync.
	/// 
	/// Async service calls are a great for GUI apps as they can be called without blocking the UI thread.
	/// They are also great for performance as no time is spent on blocking IO calls.
	/// </summary>
	[TestFixture]
	public class AsyncRestClientTests
	{
		public const string WebServiceHostUrl = "http://localhost:8080/";
		private const string ReadmeFileContents = "THIS IS A README FILE";
		private const string ReplacedFileContents = "THIS README FILE HAS BEEN REPLACED";
		private const string TestUploadFileContents = "THIS FILE IS USED FOR UPLOADING IN TESTS";
		public string FilesRootDir;

		RestFilesHttpListener appHost;

		[TestFixtureSetUp]
		public void TextFixtureSetUp()
		{
			appHost = new RestFilesHttpListener();
			appHost.Init();
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			if (appHost != null) appHost.Dispose();
			appHost = null;
		}

		[SetUp]
		public void OnBeforeEachTest()
		{
			FilesRootDir = appHost.Config.RootDirectory;
			if (Directory.Exists(FilesRootDir))
			{
				Directory.Delete(FilesRootDir, true);
			}
			Directory.CreateDirectory(FilesRootDir + "SubFolder");
			Directory.CreateDirectory(FilesRootDir + "SubFolder2");
			File.WriteAllText(Path.Combine(FilesRootDir, "README.txt"), ReadmeFileContents);
			File.WriteAllText(Path.Combine(FilesRootDir, "TESTUPLOAD.txt"), TestUploadFileContents);
		}

		public IRestClientAsync CreateAsyncRestClient()
		{
			return new JsonServiceClient(WebServiceHostUrl);  //Best choice for Ajax web apps, faster than XML
			//return new XmlServiceClient(WebServiceHostUrl); //Ubiquitous structured data format best for supporting non .NET clients
			//return new JsvServiceClient(WebServiceHostUrl); //Fastest, most compact and resilient format great for .NET to .NET client / server
		}

		private static void FailOnAsyncError<T>(T response, Exception ex)
		{
			Assert.Fail(ex.Message);
		}

		[Test]
		public void Can_GetAsync_to_retrieve_existing_file()
		{
			var restClient = CreateAsyncRestClient();

			FilesResponse response = null;
			restClient.GetAsync<FilesResponse>("files/README.txt",
				r => response = r, FailOnAsyncError);

			Thread.Sleep(1000);

			Assert.That(response.File.Contents, Is.EqualTo("THIS IS A README FILE"));
		}

		[Test]
		public void Can_GetAsync_to_retrieve_existing_folder_listing()
		{
			var restClient = CreateAsyncRestClient();

			FilesResponse response = null;
			restClient.GetAsync<FilesResponse>("files/",
				r => response = r, FailOnAsyncError);

			Thread.Sleep(1000);

			Assert.That(response.Directory.Folders.Count, Is.EqualTo(2));
			Assert.That(response.Directory.Files.Count, Is.EqualTo(2));
		}

		[Test]
		public void Can_PostAsync_to_path_without_uploaded_files_to_create_a_new_Directory()
		{
			var restClient = CreateAsyncRestClient();

			FilesResponse response = null;
			restClient.PostAsync<FilesResponse>("files/SubFolder/NewFolder",
				new Files(),
				r => response = r, FailOnAsyncError);

			Thread.Sleep(1000);

			Assert.That(Directory.Exists(FilesRootDir + "SubFolder/NewFolder"));
		}

		[Test]
		public void Can_WebRequest_POST_upload_file_to_save_new_file_and_create_new_Directory()
		{
			var webRequest = WebRequest.Create(WebServiceHostUrl + "files/UploadedFiles/");

			var fileToUpload = new FileInfo(FilesRootDir + "TESTUPLOAD.txt");
			webRequest.UploadFile(fileToUpload, MimeTypes.GetMimeType(fileToUpload.Name));

			Assert.That(Directory.Exists(FilesRootDir + "UploadedFiles"));
			Assert.That(File.ReadAllText(FilesRootDir + "UploadedFiles/TESTUPLOAD.txt"),
						Is.EqualTo(TestUploadFileContents));
		}

		[Test]
		public void Can_RestClient_POST_upload_file_to_save_new_file_and_create_new_Directory()
		{
			var restClient = (IRestClient) CreateAsyncRestClient();

			var fileToUpload = new FileInfo(FilesRootDir + "TESTUPLOAD.txt");
			restClient.PostFile<FilesResponse>("files/UploadedFiles/", 
				fileToUpload, MimeTypes.GetMimeType(fileToUpload.Name));

			Assert.That(Directory.Exists(FilesRootDir + "UploadedFiles"));
			Assert.That(File.ReadAllText(FilesRootDir + "UploadedFiles/TESTUPLOAD.txt"),
						Is.EqualTo(TestUploadFileContents));
		}

		[Test]
		public void Can_PutAsync_to_replace_text_content_of_an_existing_file()
		{
			var restClient = CreateAsyncRestClient();

			FilesResponse response = null;
			restClient.PutAsync<FilesResponse>("files/README.txt",
				new Files { TextContents = ReplacedFileContents },
				r => response = r, FailOnAsyncError);

			Thread.Sleep(1000);

			Assert.That(File.ReadAllText(FilesRootDir + "README.txt"),
						Is.EqualTo(ReplacedFileContents));
		}

		[Test]
		public void Can_DeleteAsync_to_replace_text_content_of_an_existing_file()
		{
			var restClient = CreateAsyncRestClient();

			FilesResponse response = null;
			restClient.DeleteAsync<FilesResponse>("files/README.txt",
				r => response = r, FailOnAsyncError);

			Thread.Sleep(1000);

			Assert.That(!File.Exists(FilesRootDir + "README.txt"));
		}


		/* 
		 * Error Handling Tests
		 */
		[Test]
		public void GET_a_file_that_doesnt_exist_throws_a_404_FileNotFoundException()
		{
			var restClient = CreateAsyncRestClient();

			WebServiceException webEx = null;
			FilesResponse response = null;

			restClient.GetAsync<FilesResponse>("files/UnknownFolder",
			   r => response = r,
			   (r, ex) =>
			   {
				   response = r;
				   webEx = (WebServiceException)ex;
			   });

			Thread.Sleep(1000);

			Assert.That(webEx.StatusCode, Is.EqualTo(404));
			Assert.That(response.ResponseStatus.ErrorCode, Is.EqualTo(typeof(FileNotFoundException).Name));
			Assert.That(response.ResponseStatus.Message, Is.EqualTo("Could not find: UnknownFolder"));
		}

		[Test]
		public void POST_to_an_existing_file_throws_a_500_NotSupportedException()
		{
			var restClient = (IRestClient)CreateAsyncRestClient();

			var fileToUpload = new FileInfo(FilesRootDir + "TESTUPLOAD.txt");

			try
			{
				var response = restClient.PostFile<FilesResponse>("files/README.txt",
					fileToUpload, MimeTypes.GetMimeType(fileToUpload.Name));

				Assert.Fail("Should fail with NotSupportedException");
			}
			catch (WebServiceException webEx)
			{
				Assert.That(webEx.StatusCode, Is.EqualTo(500));
				var response = (FilesResponse)webEx.ResponseDto;
				Assert.That(response.ResponseStatus.ErrorCode, Is.EqualTo(typeof(NotSupportedException).Name));
				Assert.That(response.ResponseStatus.Message,
					Is.EqualTo("POST only supports uploading new files. Use PUT to replace contents of an existing file"));
			}
		}

		[Test]
		public void PUT_to_replace_a_non_existing_file_throws_404()
		{
			var restClient = CreateAsyncRestClient();

			WebServiceException webEx = null;
			FilesResponse response = null;

			restClient.PutAsync<FilesResponse>("files/non-existing-file.txt",
			   new Files { TextContents = ReplacedFileContents },
			   r => response = r,
			   (r, ex) =>
			   {
				   response = r;
				   webEx = (WebServiceException)ex;
			   });

			Thread.Sleep(1000);

			Assert.That(webEx.StatusCode, Is.EqualTo(404));
			Assert.That(response.ResponseStatus.ErrorCode, Is.EqualTo(typeof(FileNotFoundException).Name));
			Assert.That(response.ResponseStatus.Message, Is.EqualTo("Could not find: non-existing-file.txt"));
		}

		[Test]
		public void DELETE_a_non_existing_file_throws_404()
		{
			var restClient = CreateAsyncRestClient();

			WebServiceException webEx = null;
			FilesResponse response = null;

			restClient.DeleteAsync<FilesResponse>("files/non-existing-file.txt",
			   r => response = r,
			   (r, ex) =>
			   {
				   response = r;
				   webEx = (WebServiceException)ex;
			   });

			Thread.Sleep(1000);

			Assert.That(webEx.StatusCode, Is.EqualTo(404));
			Assert.That(response.ResponseStatus.ErrorCode, Is.EqualTo(typeof(FileNotFoundException).Name));
			Assert.That(response.ResponseStatus.Message, Is.EqualTo("Could not find: non-existing-file.txt"));
		}

	}
}