using WebLearning.Contract.Dtos.AnswerMonthly;
using WebLearning.Contract.Dtos.CorrectAnswerMonthly;
using WebLearning.Contract.Dtos.OptionMonthly;
using WebLearning.Contract.Dtos.Quiz;

namespace WebLearning.Contract.Dtos.Question
{
    public class QuestionMonthlyDto
    {
        public Guid Id { get; set; }

        public Guid QuizMonthlyId { get; set; }

        public bool Active { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }

        public string CorrectAnswer { get; set; }

        public string Alias { get; set; }
        public int Point { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual QuizMonthlyDto QuizMonthlyDto { get; set; }

        public virtual ICollection<AnswerMonthlyDto> AnswerMonthlyDtos { get; set; }

        public virtual ICollection<CorrectAnswerMonthlyDto> CorrectAnswerMonthlyDtos { get; set; }

        public virtual ICollection<OptionMonthlyDto> OptionMonthlyDtos { get; set; }

    }
}
