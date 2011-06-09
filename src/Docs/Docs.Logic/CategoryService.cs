using System.Collections.Generic;
using ServiceStack.Common.Extensions;
using ServiceStack.ServiceInterface;

namespace Docs.Logic
{
	public class Category
	{
		public string Name { get; set; }
	}

	public class CategoryResponse
	{
		public string Name { get; set; }

		public List<Page> Results { get; set; }
	}

	public class CategoryService : RestServiceBase<Category>
	{
		public PageManager PageManager { get; set; }

		public override object OnGet(Category request)
		{
			List<Page> pages = null;
			if (!request.Name.IsNullOrEmpty())
				PageManager.CategoriesMap.TryGetValue(request.Name, out pages);

			return new CategoryResponse {
				Name = request.Name,
				Results = pages ?? new List<Page>()
			};
		}
	}
}