using ServiceStack;

namespace RedisStackOverflow.ServiceModel
{
    /// <summary>
    /// Define your ServiceStack web service request (i.e. the Request DTO).
    /// </summary> 
    [Route("/users")]
    public class User
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    public class UserResponse
    {
        public User Result { get; set; }
    }


    /// <summary>
    /// Define your ServiceStack web service request (i.e. the Request DTO).
    /// </summary> 
    [Route("/users/{UserId}/stats")]
    public class UserStats
    {
        public long UserId { get; set; }
    }

    public class UserStat
    {
        public long UserId { get; set; }
        public long QuestionsCount { get; set; }
        public long AnswersCount { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    public class UserStatsResponse
    {
        public UserStat Result { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service request (i.e. the Request DTO).
    /// </summary> 
    [Route("/users/{UserId}/questionvotes/{QuestionId}")]
    [Route("/users/{UserId}/answervotes/{AnswerId}")]
    public class UserVotes
    {
        public long UserId { get; set; }
        public long? QuestionId { get; set; }
        public long? AnswerId { get; set; }
        public string Direction { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    public class UserVotesResponse { }
}