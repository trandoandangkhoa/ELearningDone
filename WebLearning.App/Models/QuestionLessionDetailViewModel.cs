using WebLearning.Contract.Dtos.CorrectAnswerLession;
using WebLearning.Contract.Dtos.OptionLession;
using WebLearning.Contract.Dtos.Question;

namespace WebLearning.App.Models
{
    public class QuestionLessionDetailViewModel
    {
        public QuestionLessionDto QuestionLessionDto { get; set; }

        public List<OptionLessionDto> OptionLessionDtos { get; set; }

        public List<CorrectAnswerLessionDto> CorrectAnswerLessionDtos { get; set; }
    }
}
