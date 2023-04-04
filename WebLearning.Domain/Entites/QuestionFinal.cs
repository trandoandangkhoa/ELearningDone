using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class QuestionFinal
    {
        [Key]
        public Guid Id { get; set; }

        public string Code { get; set; }


        [ForeignKey("QuizCourse")]
        public Guid QuizCourseId { get; set; }

        public bool Active { get; set; }

        public string Name { get; set; }

        public int Point { get; set; }

        public string Alias { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual QuizCourse QuizCourse { get; set; }

        public virtual ICollection<OptionCourse> Options { get; set; }

        public virtual ICollection<CorrectAnswerCourse> CorrectAnswers { get; set; }
    }
}
