namespace WebLearning.Contract.Dtos.Question
{
    public class UpdateQuestionCourseDto
    {
        public bool Active { get; set; }
        public string Name { get; set; }
        public int Point { get; set; }
        public string Alias { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
