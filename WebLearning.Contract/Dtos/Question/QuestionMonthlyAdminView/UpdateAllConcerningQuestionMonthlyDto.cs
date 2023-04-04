using WebLearning.Contract.Dtos.CorrectAnswerMonthly;
using WebLearning.Contract.Dtos.OptionMonthly;

namespace WebLearning.Contract.Dtos.Question.QuestionMonthlyAdminView
{
    public class UpdateAllConcerningQuestionMonthlyDto
    {
        public Guid[] OptionMonthlyId { get; set; }

        public Guid[] CorrectAnswerId { get; set; }

        public string[] OptionMonthlys { get; set; }

        public bool[] CorrectAnswers { get; set; }

        public string[] NewOptionMonthlys { get; set; }

        public bool[] NewCorrectAnswers { get; set; }

        public UpdateQuestionMonthlyDto UpdateQuestionMonthly { get; set; }

        public UpdateCorrectAnswerMonthlyDto UpdateCorrectAnswerMonthly { get; set; }

        public UpdateOptionMonthlyDto UpdateOptionMonthly { get; set; }

        public OptionAndCorrectMonthlyDto OptionAndCorrectMonthlyDto { get; set; } = new OptionAndCorrectMonthlyDto();

        public List<NewOptionAndCorrectMonthlyDto> NewOptionAndCorrectMonthlyDtos { get; set; } = new List<NewOptionAndCorrectMonthlyDto>();

    }

    public class OptionAndCorrectMonthlyDto
    {
        public List<OptionMonthlyDto> OptionMonthlyDtos { get; set; } = new List<OptionMonthlyDto>();

        public List<CorrectAnswerMonthlyDto> CorrectAnswerMonthlyDtos { get; set; } = new List<CorrectAnswerMonthlyDto>();

    }
    public class NewOptionAndCorrectMonthlyDto
    {
        public string NewOptionMonthlys { get; set; }
        public bool NewCorrectAnswers { get; set; }
    }

}
