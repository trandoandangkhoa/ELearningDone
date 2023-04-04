using WebLearning.Contract.Dtos.AnswerLession;
using WebLearning.Contract.Dtos.HistorySubmit;
using WebLearning.Contract.Dtos.Lession;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.ReportScore;

namespace WebLearning.Contract.Dtos.Quiz
{
    public class QuizlessionDto
    {
        public Guid ID { get; set; }
        public Guid LessionId { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public int TimeToDo { get; set; }
        public int ScorePass { get; set; }

        public int SortItem { get; set; }
        public string Alias { get; set; }
        public bool Notify { get; set; }

        public string DescNotify { get; set; }
        public LessionDto LessionDto { get; set; }

        public virtual ICollection<QuestionLessionDto> QuestionLessionDtos { get; set; }

        public virtual ICollection<HistorySubmitLessionDto> HistorySubmitLessionDtos { get; set; }

        public virtual ICollection<AnswerLessionDto> AnswerLessionDtos { get; set; }

        public virtual ICollection<ReportScoreLessionDto> ReportScoreLessionDtos { get; set; }

    }
}
