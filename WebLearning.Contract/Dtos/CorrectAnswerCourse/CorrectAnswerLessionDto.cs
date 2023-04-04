namespace WebLearning.Contract.Dtos.CorrectAnswerCourse
{
    public class CorrectAnswerCourseDto
    {
        public Guid Id { get; set; }

        public Guid QuestionCourseId { get; set; }

        public Guid OptionCourseId { get; set; }

        public string CorrectAnswer { get; set; }

        public bool Correct { get; set; }
    }
}
