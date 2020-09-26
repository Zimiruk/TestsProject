using Common.Models.TestComponents;
using System.Collections.Generic;
using ViewModel.Models;

namespace ViewModel.Utility
{
    public static class ViewToDataConverter
    {
        public static Question QuestionConverter(QuestionView questionView)
        {
            Question question = new Question();
            question.Content = questionView.Content;
            question.Answers = new List<Answer>();

            foreach (AnswerView answer in questionView.Answers)
            {
                question.Answers.Add(new 
                    Answer { Content = answer.Content, IsItRight = answer.IsRight });
            }

            return question;
        }
    }
}