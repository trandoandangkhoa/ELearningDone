using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class AnswerMonthly
    {
        public Guid Id { get; set; }

        [ForeignKey("QuestionMonthly")]
        public Guid QuestionMonthlyId { get; set; }

        public Guid QuizMonthlyId { get; set; }

        public string AccountName { get; set; }

        public string OwnAnswer { get; set; }


        public int CheckBoxId { get; set; }
        public bool Checked { get; set; }
        public virtual QuestionMonthly QuestionMonthly { get; set; }
    }
}
