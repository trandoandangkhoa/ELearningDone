using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class HistorySubmitMonthly
    {
        [Key]
        public Guid Id { get; set; }


        [ForeignKey("QuizMonthly")]
        public Guid QuizMonthlyId { get; set; }

        public string AccountName { get; set; }

        public DateTime DateCompoleted { get; set; }

        public bool Submit { get; set; }

        public int TotalScore { get; set; }

        public virtual ICollection<QuizMonthly> QuizMonthlies { get; set; }
    }
}
