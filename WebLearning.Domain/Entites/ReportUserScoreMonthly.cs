using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class ReportUserScoreMonthly
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("QuizMonthly")]
        public Guid QuizMonthlyId { get; set; }

        public Guid RoleId { get; set; }


        public string UserName { get; set; }
        public string FullName { get; set; }

        public DateTime CompletedDate { get; set; }

        public int TotalScore { get; set; }

        public bool Passed { get; set; }
        public string IpAddress { get; set; }

        public virtual ICollection<QuizMonthly> QuizMonthlies { get; set; }
    }
}
