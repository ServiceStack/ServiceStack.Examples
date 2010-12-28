using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceStack.Questions.ServiceModel
{
	public class Answer
	{
		public int Id { get; set; }
		public string QuestionId { get; set; }
		public string UserId { get; set; }
		public string Content { get; set; }
	}
}