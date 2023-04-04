using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class OptionCourse
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("QuestionFinal")]
        public Guid QuestionFinalId { get; set; }

        public string Name { get; set; }
    }
}
