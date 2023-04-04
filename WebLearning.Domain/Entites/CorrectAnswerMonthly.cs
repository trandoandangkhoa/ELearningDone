using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class CorrectAnswerMonthly
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("QuestionMonthly")]
        public Guid QuestionMonthlyId { get; set; }

        public Guid OptionMonthlyId { get; set; }

        public string CorrectAnswer { get; set; }
    }
}
