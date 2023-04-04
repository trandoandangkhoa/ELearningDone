using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.Quiz;

namespace WebLearning.Contract.Dtos.HistorySubmit
{
    public class HistorySubmitLessionDto
    {
        public Guid Id { get; set; }

        public Guid QuizLessionId { get; set; }

        public Guid QuestionLessionId { get; set; }

        public string AccountName { get; set; }

        public DateTime DateCompoleted { get; set; }

        public bool Submit { get; set; }

        public int TotalScore { get; set; }

        public virtual ICollection<QuizlessionDto> QuizlessionDtos { get; set; }
        public virtual ICollection<QuestionLessionDto> QuestionLessionDtos { get; set; }
    }
}
