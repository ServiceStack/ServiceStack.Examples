using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using ServiceStack.ServiceClient.Web;
using ServiceStack.Text;
using ServiceStack.Text.Common;
using ServiceStack.Service;
using RestFiles.ServiceModel.Operations;
using RestFiles.ServiceModel.Types;

namespace RestFilesClient
{
	public class Application
	{
		static void Main (string[] args)
		{
			UIApplication.Main (args);
		}
	}

	// The name AppDelegate is referenced in the MainWindow.xib file.
	public partial class AppDelegate : UIApplicationDelegate
	{
		JsonServiceClient gateway = new JsonServiceClient("http://servicestack.net/RestFiles");
		//JsvServiceClient gateway = new JsvServiceClient("http://servicestack.net/RestFiles");
		
		// This method is invoked when the application has loaded its UI and its ready to run
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// If you have defined a view, add it here:
			// window.AddSubview (navigationController.View);
			
			window.MakeKeyAndVisible();

			return true; 
		}

		// This method is required in iPhoneOS 3.0
		public override void OnActivated (UIApplication application)
		{
		}
		
		partial void getFilesAtPath (MonoTouch.UIKit.UIButton sender)
		{
			//calling sync web service
			var response = gateway.Get<FilesResponse>(txtPath.Text);
			txtResults.Text = response.Dump();

			var alertView = new UIAlertView("Alert", "getFiles Sync: " + txtPath.Text, null, "Cancel");
			alertView.Show();			
		}
		
		partial void getFilesAtPathAsync (MonoTouch.UIKit.UIButton sender)
		{
			//calling async web service
			gateway.GetAsync<FilesResponse>(txtPath.Text, 
				r => InvokeOnMainThread(() => txtResults.Text = r.Dump()), 
				null);
		}
		
	}
}

