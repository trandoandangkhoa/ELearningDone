using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class QuestionMonthly
    {
        [Key]
        public Guid Id { get; set; }

        public string Code { get; set; }


        [ForeignKey("QuizMonthly")]
        public Guid QuizMonthlyId { get; set; }

        public bool Active { get; set; }
        public string Name { get; set; }

        public int Point { get; set; }
        public string Alias { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual QuizMonthly QuizMonthly { get; set; }

        public virtual ICollection<OptionMonthly> Options { get; set; }

        public virtual ICollection<CorrectAnswerMonthly> CorrectAnswers { get; set; }
    }
}
