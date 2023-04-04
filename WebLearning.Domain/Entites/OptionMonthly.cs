using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class OptionMonthly
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("QuestionMonthly")]
        public Guid QuestionMonthlyId { get; set; }

        public string Name { get; set; }
    }
}
