using WebLearning.Contract.Dtos.CorrectAnswerLession;
using WebLearning.Contract.Dtos.OptionLession;

namespace WebLearning.Contract.Dtos.Question.QuestionLessionAdminView
{
    public class UpdateAllConcerningQuestionLesstionDto
    {
        public Guid[] OptionLessionId { get; set; }

        public Guid[] CorrectAnswerId { get; set; }

        public string[] OptionLessions { get; set; }

        public bool[] CorrectAnswers { get; set; }

        public string[] NewOptionLessions { get; set; }

        public bool[] NewCorrectAnswers { get; set; }

        public UpdateQuestionLessionDto UpdateQuestionLession { get; set; }

        public UpdateCorrectAnswerLessionDto UpdateCorrectAnswerLession { get; set; }

        public UpdateOptionLessionDto UpdateOptionLession { get; set; }

        public OptionAndCorrectLessionDto OptionAndCorrectLessionDto { get; set; } = new OptionAndCorrectLessionDto();

        public List<NewOptionAndCorrectLessionDto> NewOptionAndCorrectLessionDtos { get; set; } = new List<NewOptionAndCorrectLessionDto>();

    }

    public class OptionAndCorrectLessionDto
    {
        public List<OptionLessionDto> OptionLessionDtos { get; set; } = new List<OptionLessionDto>();

        public List<CorrectAnswerLessionDto> CorrectAnswerLessionDtos { get; set; } = new List<CorrectAnswerLessionDto>();

    }
    public class NewOptionAndCorrectLessionDto
    {
        public string NewOptionLessions { get; set; }
        public bool NewCorrectAnswers { get; set; }
    }

}
