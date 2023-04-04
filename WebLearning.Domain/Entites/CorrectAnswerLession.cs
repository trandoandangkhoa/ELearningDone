using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class CorrectAnswerLession
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("QuestionLession")]
        public Guid QuestionLessionId { get; set; }

        public Guid OptionLessionId { get; set; }

        public string CorrectAnswer { get; set; }
    }
}
