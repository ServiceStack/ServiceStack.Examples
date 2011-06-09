@var Category = Model.Name
@var Title = Category

#### Category Pages

@foreach page in Model.Results {
  - [@page.Name](@page.AbsoluteUrl)
}
