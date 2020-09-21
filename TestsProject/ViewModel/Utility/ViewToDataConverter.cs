using Common.Models;
using System.Collections.Generic;
using ViewModel.Models;

namespace ViewModel.Utility
{
    public static class ViewToDataConverter
    {
        public static Question QuestionConverter(QuestionView questionView)
        {
            Question question = new Question();
            question.QuestionContent = questionView.QuestionContent;
            question.Answers = new List<Answer>();

            foreach (AnswerView answer in questionView.Answers)
            {
                question.Answers.Add(new Answer { Content = answer.AnswerContent, IsItRight = answer.IsRight });
            }

            return question;
        }
    }
}
