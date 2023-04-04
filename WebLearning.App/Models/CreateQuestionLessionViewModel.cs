using WebLearning.Contract.Dtos.CorrectAnswerLession;
using WebLearning.Contract.Dtos.OptionLession;
using WebLearning.Contract.Dtos.Question;

namespace WebLearning.App.Models
{
    public class CreateQuestionLessionViewModel
    {
        public CreateQuestionLessionDto CreateQuestionLessionDto { get; set; }
        public CreateOptionLessionDto CreateOptionLessionDto { get; set; }
        public CreateCorrectAnswerLessionDto CreateCorrectAnswerLessionDto { get; set; }

        public QuestionLessionDetailViewModel QuestionLessionDetailViewModel { get; set; }
    }
}
