using System;
using System.Collections.Generic;
using ServiceStack;

namespace Docs.Logic
{
	public class Search
	{
		public string Query { get; set; }
	}

	public class SearchResponse : IHasResponseStatus
	{
		public SearchResponse()
		{
			this.Results = new List<Page>();
		}

		public string Query { get; set; }

		public List<Page> Results { get; set; }

		public ResponseStatus ResponseStatus { get; set; }
	}

	public class SearchService : Service
	{
		public PageManager PageManager { get; set; }

		public object Get(Search request)
		{
			var results = new List<Page>();
			foreach (var page in PageManager.Pages)
			{
				var contents = page.GetContent().StripMarkdownMarkup();

				var pos = 0;
				if ((pos = contents.IndexOf(request.Query, StringComparison.CurrentCultureIgnoreCase)) != -1)
				{
					var resultPage = page.DeepClone();
					var snippet = GetSnippet(contents, pos);
					resultPage.Content = snippet.Replace(request.Query, "**" + request.Query + "**");
					results.Add(resultPage);
				}
			}
			return new SearchResponse {
				Query = request.Query,
				Results = results
			};
		}

		private static string GetSnippet(string contents, int pos)
		{
			var startPos = pos - 50;
			var endPos = pos + 100;
			if (endPos >= contents.Length)
			{
				endPos = contents.Length - 1;
				startPos = endPos - 100;
			}
			if (startPos < 0) startPos = 0;
			if (contents[startPos] != ' ')
			{
				var wordBoundaryPos = contents.LastIndexOf(' ', startPos);
				if (wordBoundaryPos != -1) startPos = wordBoundaryPos + 1;
			}
			if (contents[endPos] != ' ')
			{
				var wordBoundaryPos = contents.IndexOf(' ', endPos);
				if (wordBoundaryPos != -1) endPos = wordBoundaryPos;
			}

			var snippet = contents.Substring(startPos, endPos - startPos);

			return snippet;
		}
	}
}