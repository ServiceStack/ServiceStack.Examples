@var Title = "Search results for " + Model.Query

@if (Model.Results.Count == 0) {

#### Your search did not match any documents.

## Suggestions:

  - Make sure all words are spelled correctly.
  - Try different keywords.
  - Try more general keywords.
  - Try fewer keywords.
} else {

#### Showing Results 1 - @Model.Results.Count

^<div id="searchresults">

@foreach page in Model.Results {
### @page.Category &gt; [@page.Name](@page.AbsoluteUrl)
@page.Content
}

^</div>

}