using WebLearning.Contract.Dtos.Question;

namespace WebLearning.Contract.Dtos.AnswerCourse
{
    public class CreateAnswerCourseDto
    {
        public Guid Id { get; set; }

        public Guid QuestionCourseId { get; set; }

        public Guid QuizCourseId { get; set; }

        public string AccountName { get; set; }

        public string OwnAnswer { get; set; }

        public int CheckBoxId { get; set; }

        public bool Checked { get; set; }
        public virtual QuestionCourseDto QuestionCourseDto { get; set; }

    }
}
