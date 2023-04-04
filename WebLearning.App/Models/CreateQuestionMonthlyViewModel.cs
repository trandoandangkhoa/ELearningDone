using WebLearning.Contract.Dtos.CorrectAnswerMonthly;
using WebLearning.Contract.Dtos.OptionMonthly;
using WebLearning.Contract.Dtos.Question;

namespace WebLearning.App.Models
{
    public class CreateQuestionMonthlyViewModel
    {
        public CreateQuestionMonthlyDto CreateQuestionMonthlyDto { get; set; }
        public CreateOptionMonthlyDto CreateOptionMonthlyDto { get; set; }

        public CreateCorrectAnswerMonthlyDto CreateCorrectAnswerMonthlyDto { get; set; }
    }
}
