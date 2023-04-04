using WebLearning.Contract.Dtos.CorrectAnswerLession;
using WebLearning.Contract.Dtos.OptionLession;

namespace WebLearning.Contract.Dtos.Question.QuestionLessionAdminView
{
    public class CreateAllConcerningQuestionLessionDto
    {

        public CreateQuestionLessionDto CreateQuestionLessionDto { get; set; } = new CreateQuestionLessionDto();

        public CreateOptionLessionDto CreateOptionLessionDto { get; set; } = new CreateOptionLessionDto();

        public CreateCorrectAnswerLessionDto CreateCorrectAnswerLessionDto { get; set; } = new CreateCorrectAnswerLessionDto();

        public string[] OptionLessions { get; set; }

        public bool[] CorrectAnswers { get; set; }

        public List<OptionAndCorrectLession> OptionAndCorrectLessions { get; set; } = new List<OptionAndCorrectLession>();

        public QuestionLessionDto QuestionLessionDto { get; set; }
    }

    public class OptionAndCorrectLession
    {
        public string OptionLessions { get; set; }
        public bool CorrectAnswers { get; set; }
    }


}
