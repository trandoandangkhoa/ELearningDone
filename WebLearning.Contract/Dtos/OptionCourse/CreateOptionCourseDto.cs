namespace WebLearning.Contract.Dtos.OptionCourse
{
    public class CreateOptionCourseDto
    {
        public Guid Id { get; set; }

        public Guid QuestionFinalId { get; set; }

        public string Name { get; set; }
    }
}
