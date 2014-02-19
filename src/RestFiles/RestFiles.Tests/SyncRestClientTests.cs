using System;
using System.IO;
using NUnit.Framework;
using RestFiles.ServiceModel;
using ServiceStack;

/* For syntax highlighting and better readability of this file, view it on GitHub:
 * https://github.com/ServiceStack/ServiceStack.Examples/blob/master/src/RestFiles/RestFiles.Tests/SyncRestClientTests.cs
 */

namespace RestFiles.Tests
{
	/// <summary>
	/// These test show how you can call ServiceStack REST web services synchronously using an IRestClient.
	/// 
	/// Sync IO calls are the most familiar calling convention for .NET developers as it provides the simplest 
	/// API to use and requires the least code and effort as it allows you to program sequentially 
	/// </summary>
	[TestFixture]
	public class SyncRestClientTests
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
			//Setup the files directory with some test files and folders
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

		/// <summary>
		/// Choose your favourite format to run tests with
		/// </summary>
		public IRestClient CreateRestClient()
		{
			return new JsonServiceClient(WebServiceHostUrl);  //Best choice for Ajax web apps, 3x faster than XML
			//return new XmlServiceClient(WebServiceHostUrl); //Ubiquitous structured data format best for supporting non .NET clients
			//return new JsvServiceClient(WebServiceHostUrl); //Fastest, most compact and resilient format great for .NET to .NET client > server
		}

		[Test]
		public void Can_Get_to_retrieve_existing_file()
		{
			var restClient = CreateRestClient();

			var response = restClient.Get<FilesResponse>("files/README.txt");

			Assert.That(response.File.Contents, Is.EqualTo("THIS IS A README FILE"));
		}

		[Test]
		public void Can_Get_to_retrieve_existing_folder_listing()
		{
			var restClient = CreateRestClient();

			var response = restClient.Get<FilesResponse>("files/");

			Assert.That(response.Directory.Folders.Count, Is.EqualTo(2));
			Assert.That(response.Directory.Files.Count, Is.EqualTo(2));
		}

		[Test]
		public void Can_Post_to_path_without_uploaded_files_to_create_a_new_Directory()
		{
			var restClient = CreateRestClient();

			var response = restClient.Post<FilesResponse>("files/SubFolder/NewFolder", new Files());

			Assert.That(Directory.Exists(FilesRootDir + "SubFolder/NewFolder"));
		}

		[Test]
		public void Can_WebRequest_POST_upload_file_to_save_new_file_and_create_new_Directory()
		{
			var restClient = CreateRestClient();

			var fileToUpload = new FileInfo(FilesRootDir + "TESTUPLOAD.txt");

			var response = restClient.PostFile<FilesResponse>("files/UploadedFiles/", 
				fileToUpload, MimeTypes.GetMimeType(fileToUpload.Name));

			Assert.That(Directory.Exists(FilesRootDir + "UploadedFiles"));
			Assert.That(File.ReadAllText(FilesRootDir + "UploadedFiles/TESTUPLOAD.txt"),
			            Is.EqualTo(TestUploadFileContents));
		}

		[Test]
		public void Can_Put_to_replace_text_content_of_an_existing_file()
		{
			var restClient = CreateRestClient();

			var response = restClient.Put<FilesResponse>(WebServiceHostUrl + "files/README.txt",
				new Files { TextContents = ReplacedFileContents });

			Assert.That(File.ReadAllText(FilesRootDir + "README.txt"),
						Is.EqualTo(ReplacedFileContents));
		}

		[Test]
		public void Can_Delete_to_replace_text_content_of_an_existing_file()
		{
			var restClient = CreateRestClient();

			var response = restClient.Delete<FilesResponse>("files/README.txt");

			Assert.That(!File.Exists(FilesRootDir + "README.txt"));
		}


		/* 
		 * Error Handling Tests
		 */
		[Test]
		public void GET_a_file_that_doesnt_exist_throws_a_404_FileNotFoundException()
		{
			var restClient = CreateRestClient();

			try
			{
				var response = restClient.Get<FilesResponse>(WebServiceHostUrl + "files/UnknownFolder");

				Assert.Fail("Should fail with 404 FileNotFoundException");
			}
			catch (WebServiceException webEx)
			{
				Assert.That(webEx.StatusCode, Is.EqualTo(404));
				var response = (FilesResponse)webEx.ResponseDto;
				Assert.That(response.ResponseStatus.ErrorCode, Is.EqualTo(typeof(FileNotFoundException).Name));
				Assert.That(response.ResponseStatus.Message, Is.EqualTo("Could not find: UnknownFolder"));
			}
		}

		[Test]
		public void POST_to_an_existing_file_throws_a_500_NotSupportedException()
		{
			var restClient = CreateRestClient();

			var fileToUpload = new FileInfo(FilesRootDir + "TESTUPLOAD.txt");

			try
			{
				var response = restClient.PostFile<FilesResponse>(WebServiceHostUrl + "files/README.txt",
					fileToUpload, MimeTypes.GetMimeType(fileToUpload.Name));

				Assert.Fail("Should fail with NotSupportedException");
			}
			catch (WebServiceException webEx)
			{
                Assert.That(webEx.StatusCode, Is.EqualTo(405));
				var response = (FilesResponse)webEx.ResponseDto;
				Assert.That(response.ResponseStatus.ErrorCode, Is.EqualTo(typeof(NotSupportedException).Name));
				Assert.That(response.ResponseStatus.Message,
					Is.EqualTo("POST only supports uploading new files. Use PUT to replace contents of an existing file"));
			}
		}

		[Test]
		public void PUT_to_replace_a_non_existing_file_throws_404()
		{
			var restClient = CreateRestClient();

			try
			{
				var response = restClient.Put<FilesResponse>(WebServiceHostUrl + "files/non-existing-file.txt",
					new Files { TextContents = ReplacedFileContents });

				Assert.Fail("Should fail with 404 FileNotFoundException");
			}
			catch (WebServiceException webEx)
			{
				Assert.That(webEx.StatusCode, Is.EqualTo(404));
				var response = (FilesResponse)webEx.ResponseDto;
				Assert.That(response.ResponseStatus.ErrorCode, Is.EqualTo(typeof(FileNotFoundException).Name));
				Assert.That(response.ResponseStatus.Message, Is.EqualTo("Could not find: non-existing-file.txt"));
			}
		}

		[Test]
		public void DELETE_a_non_existing_file_throws_404()
		{
			var restClient = CreateRestClient();

			try
			{
				var response = restClient.Delete<FilesResponse>(WebServiceHostUrl + "files/non-existing-file.txt");

				Assert.Fail("Should fail with 404 FileNotFoundException");
			}
			catch (WebServiceException webEx)
			{
				Assert.That(webEx.StatusCode, Is.EqualTo(404));
				var response = (FilesResponse)webEx.ResponseDto;
				Assert.That(response.ResponseStatus.ErrorCode, Is.EqualTo(typeof(FileNotFoundException).Name));
				Assert.That(response.ResponseStatus.Message, Is.EqualTo("Could not find: non-existing-file.txt"));
			}
		}

	}
}