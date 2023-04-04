using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class AnswerCourse
    {
        public Guid Id { get; set; }

        [ForeignKey("QuestionFinal")]
        public Guid QuestionCourseId { get; set; }

        public Guid QuizCourseId { get; set; }

        public string AccountName { get; set; }

        public string OwnAnswer { get; set; }


        public int CheckBoxId { get; set; }
        public bool Checked { get; set; }
        public virtual QuestionFinal QuestionFinal { get; set; }
    }
}
