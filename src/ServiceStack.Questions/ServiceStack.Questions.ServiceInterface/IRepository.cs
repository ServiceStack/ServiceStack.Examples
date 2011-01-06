using System.Collections;
using System.Collections.Generic;
using ServiceStack.Redis.Generic;

namespace ServiceStack.Questions.ServiceInterface
{
	public interface IRepository
	{
		User GetOrCreateUser(User user);

		List<Question> GetAllQuestions();

		List<QuestionResult> GetRecentQuestionResults(int skip, int take);

		void StoreQuestion(Question question);

		void StoreAnswer(Answer answer);

		List<Answer> GetAnswersForQuestion(long questionId);

		void VoteQuestionUp(long userId, long questionId);

		void VoteQuestionDown(long userId, long questionId);

		void VoteAnswerUp(long userId, long answerId);

		void VoteAnswerDown(long userId, long answerId);

		QuestionStat GetQuestionStats(long questionId);

		Question GetQuestion(long questionId);

		List<User> GetUsersByIds(IEnumerable<long> userIds);
	}
}