using WebLearning.Contract.Dtos.AnswerLession;
using WebLearning.Contract.Dtos.HistorySubmit;
using WebLearning.Contract.Dtos.ReportScore;

namespace WebLearning.Contract.Dtos.Quiz
{
    public class QuizLessionViewModel
    {
        public QuizlessionDto QuizlessionDto { get; set; }

        public virtual ICollection<HistorySubmitLessionDto> HistorySubmitLessionDtos { get; set; }

        public virtual ICollection<AnswerLessionDto> AnswerLessionDtos { get; set; }

        public virtual ICollection<ReportScoreLessionDto> ReportScoreLessionDtos { get; set; }
    }
}
