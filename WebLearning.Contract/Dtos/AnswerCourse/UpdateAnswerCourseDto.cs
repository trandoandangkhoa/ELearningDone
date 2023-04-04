namespace WebLearning.Contract.Dtos.AnswerCourse
{
    public class UpdateAnswerCourseDto
    {
        public string AccountName { get; set; }

        public string OwnAnswer { get; set; }

        public int CheckBoxId { get; set; }

        public bool Checked { get; set; }
    }
}
