using WebLearning.Contract.Dtos.AnswerMonthly;
using WebLearning.Contract.Dtos.HistorySubmit;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.ReportScore;
using WebLearning.Contract.Dtos.Role;

namespace WebLearning.Contract.Dtos.Quiz
{
    public class QuizMonthlyDto
    {
        public Guid ID { get; set; }

        public Guid RoleId { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public DateTime DateCreated { get; set; }

        public int TimeToDo { get; set; }

        public int ScorePass { get; set; }
        public bool Notify { get; set; }

        public string DescNotify { get; set; }

        public bool Passed { get; set; }
        public string Alias { get; set; }
        public Guid CreatedBy { get; set; }

        public virtual RoleDto RoleDto { get; set; }

        public virtual ICollection<QuestionMonthlyDto> QuestionMonthlyDtos { get; set; }

        public virtual ICollection<HistorySubmitMonthlyDto> HistorySubmitMonthlyDtos { get; set; }

        public virtual ICollection<AnswerMonthlyDto> AnswerMonthlyDtos { get; set; }

        public virtual ICollection<ReportScoreMonthlyDto> ReportScoreMonthlyDtos { get; set; }

    }
}
