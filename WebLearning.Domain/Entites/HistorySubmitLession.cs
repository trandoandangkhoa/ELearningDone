using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class HistorySubmitLession
    {
        [Key]
        public Guid Id { get; set; }


        [ForeignKey("QuizLession")]
        public Guid QuizLessionId { get; set; }

        public string AccountName { get; set; }

        public DateTime DateCompoleted { get; set; }

        public bool Submit { get; set; }

        public int TotalScore { get; set; }

        public virtual ICollection<QuizLession> QuizLessions { get; set; }
    }
}
