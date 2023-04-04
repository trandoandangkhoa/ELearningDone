using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class OptionLession
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("QuestionLession")]
        public Guid QuestionLessionId { get; set; }
        public string Name { get; set; }
    }
}
