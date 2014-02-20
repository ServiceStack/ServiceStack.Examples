using System.Collections.Generic;
using ServiceStack;

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

	public class CategoryService : Service
	{
		public PageManager PageManager { get; set; }

		public object Get(Category request)
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