using NUnit.Framework;
using ServiceStack;
using System.Collections.Generic;
using StarterTemplates.Common;

namespace StarterTemplates.Tests
{
	[TestFixture]
	public class TodoAppTests
	{
		[Test]
		public void Can_view_create_and_delete_TODOs_over_REST()
		{
            var restClient = new JsonServiceClient("http://localhost:37022/");

			var all = restClient.Get<List<Todo>>("/todos");
			Assert.That(all.Count, Is.EqualTo(0));

			var todo = restClient.Post<Todo>("/todos", new Todo { Content = "New TODO", Order = 1 });
			Assert.That(todo.Id, Is.GreaterThan(0));
			Assert.That(todo.Content, Is.EqualTo("New TODO"));

			all = restClient.Get<List<Todo>>("/todos");
			Assert.That(all.Count, Is.EqualTo(1));

			todo.Content = "Updated TODO";
			todo = restClient.Post<Todo>("/todos", todo);
			Assert.That(todo.Content, Is.EqualTo("Updated TODO"));

			restClient.Delete<Todo>("/todos/" + todo.Id);

			all = restClient.Get<List<Todo>>("/todos");
			Assert.That(all.Count, Is.EqualTo(0));
		}
	}
}