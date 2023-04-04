using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class ReportUserScore
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("QuizLession")]
        public Guid QuizLessionId { get; set; }

        public Guid LessionId { get; set; }


        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateTime CompletedDate { get; set; }

        public int TotalScore { get; set; }

        public bool Passed { get; set; }

        public string IpAddress { get; set; }

        public virtual ICollection<QuizLession> QuizLessions { get; set; }




    }
}
