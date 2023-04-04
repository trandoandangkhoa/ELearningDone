using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class QuizMonthly
    {
        [Key]
        public Guid ID { get; set; }

        [ForeignKey("Role")]
        public Guid RoleId { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public DateTime DateCreated { get; set; }

        public int TimeToDo { get; set; }

        public int ScorePass { get; set; }

        public bool Passed { get; set; }


        public bool Notify { get; set; }

        public string DescNotify { get; set; }

        public string Alias { get; set; }


        public Guid CreatedBy { get; set; }
        public virtual Role Role { get; set; }

        public virtual ICollection<QuestionMonthly> QuestionMonthlies { get; set; }
    }
}
