using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ServiceStack.Examples.Clients.Soap12ServiceReference;

namespace ServiceStack.Examples.Clients
{
    public partial class Soap12 : System.Web.UI.Page
    {
        private const string EndpointUri = "http://localhost:64067/ServiceStack/soap12";

        private readonly SyncReplyClient client;

        //Generated proxy when using 'Add Service Reference' on the EndpointUri above.
        //Thank WCF for the config ugliness

        public Soap12()
        {
            var binding = new WSHttpBinding
            {
                MaxReceivedMessageSize = int.MaxValue,
                HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                MaxBufferPoolSize = 524288,
            };
            binding.Security.Mode = SecurityMode.None;

            client = new SyncReplyClient(
                binding,
                new EndpointAddress(EndpointUri));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnGetFactorial_Click(object sender, EventArgs e)
        {
            litGetFactorialResult.Text = litGetFactorialError.Text = "";
            try
            {
                var longValue = long.Parse(txtGetFactorial.Text);
                var result = client.GetFactorial(new GetFactorial { ForNumber = longValue });
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
                var results = client.GetFibonacciNumbers(new GetFibonacciNumbers
                {
                    Skip = skipValue,
                    Take = takeValue,
                }).Results;

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
                var response = client.StoreNewUser(
                    new StoreNewUser
                    {
                        Email = txtStoreNewUserEmail.Text,
                        UserName = txtStoreNewUserUsername.Text,
                        Password = txtStoreNewUserPassword.Text,
                    });

                var userIdResult = response.UserId;

                if (response.ResponseStatus.ErrorCode != null)
                {
                    litStoreNewUserError.Text = response.ResponseStatus.ErrorCode.ToEnglish();
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
                client.DeleteAllUsers(new DeleteAllUsers());

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
                var userIds = new List<string>(txtGetUsersUserIds.Text.Split(','))
                    .Where(x => !string.IsNullOrEmpty(x.Trim()))
                    .Map(x => long.Parse(x.Trim())).ToArray();

                var userResults = client.GetUsers(new GetUsers { UserIds = userIds, }).Users;

                if (userResults != null && userIds.Length > 0)
                {
                    var sb = new StringBuilder();

                    foreach (User user in userResults)
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
