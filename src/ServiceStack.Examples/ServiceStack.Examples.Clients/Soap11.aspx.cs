using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ServiceStack.Common.Extensions;
using ServiceStack.Examples.Clients.Soap11ServiceReference;

namespace ServiceStack.Examples.Clients
{
	public partial class Soap11 : System.Web.UI.Page
	{
		private const string EndpointUri = "http://localhost/ServiceStack.Examples.Host.Web/ServiceStack/Soap11";

		//Generated proxy when using 'Add Service Reference' on the EndpointUri above.
		//Thank WCF for the config ugliness
		readonly SyncReplyClient client = new SyncReplyClient(
			new BasicHttpBinding
			{
				MaxReceivedMessageSize = int.MaxValue,
				HostNameComparisonMode = HostNameComparisonMode.StrongWildcard
			},
			new EndpointAddress(EndpointUri));

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void btnGetFactorial_Click(object sender, EventArgs e)
		{
			litGetFactorialResult.Text = litGetFactorialError.Text = "";
			try
			{
				var longValue = long.Parse(txtGetFactorial.Text);
				var result = client.GetFactorial(longValue);
				litGetFactorialResult.Text = result.ToString();
			}
			catch (Exception ex)
			{
				litGetFactorialError.Text = ex.Message;
			}
		}

		protected void btnGetFibonacci_Click(object sender, EventArgs e)
		{
			litGetFibonacciResult.Text = litGetFibonacciError.Text = "";
			try
			{
				var skipValue = long.Parse(txtGetFibonacciSkip.Text);
				var takeValue = long.Parse(txtGetFibonacciTake.Text);
				var results = client.GetFibonacciNumbers(skipValue, takeValue);

				litGetFibonacciResult.Text = string.Join(", ", results.ConvertAll(x => x.ToString()).ToArray());
			}
			catch (Exception ex)
			{
				litGetFibonacciError.Text = ex.Message;
			}
		}

		protected void btnStoreNewUser_Click(object sender, EventArgs e)
		{
			litStoreNewUserResult.Text = litStoreNewUserError.Text = "";
			try
			{
				long userIdResult;
				var responseStatus = client.StoreNewUser(out userIdResult, 
					txtStoreNewUserEmail.Text,
					txtStoreNewUserPassword.Text,
					txtStoreNewUserUsername.Text);

				if (responseStatus.ErrorCode != null)
				{
					litStoreNewUserError.Text = responseStatus.ErrorCode.ToEnglish();
					return;
				}

				litStoreNewUserResult.Text = "New User Id: " + userIdResult;
				var userIds = new List<string>(txtGetUsersUserIds.Text.Split(','))
              	{
              		userIdResult.ToString()
              	}.Where(x => !string.IsNullOrEmpty(x.Trim()));

				txtGetUsersUserIds.Text = string.Join(",", userIds.ToArray());
			}
			catch (Exception ex)
			{
				litStoreNewUserError.Text = ex.Message;
			}
		}

		protected void btnDeleteAllUsers_Click(object sender, EventArgs e)
		{
			litStoreNewUserResult.Text = litStoreNewUserError.Text = "";
			try
			{
				long userIdResult;
				client.DeleteAllUsers(out userIdResult);

				litStoreNewUserResult.Text = "All users were deleted.";
			}
			catch (Exception ex)
			{
				litStoreNewUserError.Text = ex.Message;
			}
		}

		protected void btnGetUsers_Click(object sender, EventArgs e)
		{
			litGetUsersResult.Text = litGetUsersError.Text = "";
			try
			{
				User[] userResults;
				var userIds = new List<string>(txtGetUsersUserIds.Text.Split(','))
					.Where(x => !string.IsNullOrEmpty(x.Trim()))
					.ConvertAll(x => long.Parse(x.Trim())).ToArray();
				
				client.GetUsers(out userResults, userIds, null);

				if (userResults != null && userIds.Length > 0)
				{
					var sb = new StringBuilder();

					foreach (var user in userResults)
					{
						sb.AppendFormat("<div class='user'>{0}<br/>{1}<br/></div>\n",
						                user.UserName, user.Password);
					}

					litGetUsersResult.Text = sb.ToString();
				}
				else
				{
					litGetUsersResult.Text = "No matching users found.";
				}
			}
			catch (Exception ex)
			{
				litGetUsersError.Text = ex.Message;
			}
		}
	}

}
