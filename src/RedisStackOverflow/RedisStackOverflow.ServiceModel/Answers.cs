using System;
using ServiceStack;

namespace RedisStackOverflow.ServiceModel
{
    /// <summary>
    /// Define your ServiceStack web service request (i.e. the Request DTO).
    /// </summary>   
    [Route("/answers")]
    public class Answers
    {
        public int UserId { get; set; }
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string Content { get; set; }
    }

    public class Answer
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Content { get; set; }
    }

    public class AnswerResult
    {
        public Answer Answer { get; set; }
        public User User { get; set; }
    }
}