using RazorEngine;

namespace PhotoGallery.Logic
{
	public class WebSecurity
	{
		public bool IsAuthenticated { get; set; }
		public string CurrentUserName { get; set; }

		public void InitializeDatabaseConnection(string photogallery, string userprofiles, string userid, string email, bool b)
		{
			//throw new System.NotImplementedException();
		}
	}

	public class PageInfo
	{
		public string Title { get; set; }
	}

	public abstract class BasePage : BasePage<dynamic>
	{
	}

	public abstract class BasePage<TModel> : RazorPageBase<TModel>
	{
		public PageInfo Page = new PageInfo();
		public WebSecurity WebSecurity = new WebSecurity();
	}
}