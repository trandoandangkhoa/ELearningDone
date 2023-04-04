using WebLearning.Contract.Dtos.AnswerLession;
using WebLearning.Contract.Dtos.CorrectAnswerLession;
using WebLearning.Contract.Dtos.OptionLession;
using WebLearning.Contract.Dtos.Quiz;

namespace WebLearning.Contract.Dtos.Question
{
    public class QuestionLessionDto
    {
        public Guid Id { get; set; }

        public Guid QuizLessionId { get; set; }

        public bool Active { get; set; }
        public string Name { get; set; }

        public int Point { get; set; }

        public string Alias { get; set; }
        public string Code { get; set; }
        public DateTime DateCreated { get; set; }
        public QuizlessionDto QuizlessionDto { get; set; }
        public virtual ICollection<AnswerLessionDto> AnswerLessionDtos { get; set; }
        public virtual ICollection<CorrectAnswerLessionDto> CorrectAnswerLessionDtos { get; set; }
        public virtual ICollection<OptionLessionDto> OptionLessionDtos { get; set; }


    }
}
