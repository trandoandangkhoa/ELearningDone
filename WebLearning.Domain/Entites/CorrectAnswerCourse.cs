using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class CorrectAnswerCourse
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("QuestionFinal")]
        public Guid QuestionCourseId { get; set; }

        public Guid OptionCourseId { get; set; }

        public string CorrectAnswer { get; set; }
    }
}
