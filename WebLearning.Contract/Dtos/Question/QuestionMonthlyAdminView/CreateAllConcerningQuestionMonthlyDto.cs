using WebLearning.Contract.Dtos.CorrectAnswerMonthly;
using WebLearning.Contract.Dtos.OptionMonthly;

namespace WebLearning.Contract.Dtos.Question.QuestionMonthlyAdminView
{
    public class CreateAllConcerningQuestionMonthlyDto
    {
        public CreateQuestionMonthlyDto CreateQuestionMonthlyDto { get; set; }

        public CreateOptionMonthlyDto CreateOptionMonthlyDto { get; set; } = new CreateOptionMonthlyDto();

        public CreateCorrectAnswerMonthlyDto CreateCorrectAnswerMonthlyDto { get; set; } = new CreateCorrectAnswerMonthlyDto();

        public string[] OptionMonthlys { get; set; }

        public bool[] CorrectAnswers { get; set; }

        public List<OptionAndCorrectMonthly> OptionAndCorrectMonthlys { get; set; } = new List<OptionAndCorrectMonthly>();

        public QuestionMonthlyDto QuestionMonthlyDto { get; set; }

    }

    public class OptionAndCorrectMonthly
    {
        public string OptionMonthlys { get; set; }
        public bool CorrectAnswers { get; set; }
    }

}
