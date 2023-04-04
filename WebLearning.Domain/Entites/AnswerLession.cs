using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class AnswerLession
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("QuestionLession")]
        public Guid QuestionLessionId { get; set; }

        public Guid QuizLessionId { get; set; }

        public string AccountName { get; set; }

        public string OwnAnswer { get; set; }

        public int CheckBoxId { get; set; }
        public bool Checked { get; set; }
        public virtual QuestionLession Question { get; set; }
    }
}
