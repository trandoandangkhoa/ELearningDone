using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class QuestionLession
    {
        [Key]
        public Guid Id { get; set; }
        public string Code { get; set; }

        [ForeignKey("QuizLession")]
        public Guid QuizLessionId { get; set; }

        public bool Active { get; set; }

        public string Name { get; set; }

        public int Point { get; set; }

        public string Alias { get; set; }

        public DateTime DateCreated { get; set; }
        public virtual QuizLession QuizLession { get; set; }

        public virtual ICollection<OptionLession> Options { get; set; }

        public virtual ICollection<CorrectAnswerLession> CorrectAnswers { get; set; }

    }
}
