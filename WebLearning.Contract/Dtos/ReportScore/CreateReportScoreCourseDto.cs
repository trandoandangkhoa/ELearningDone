namespace WebLearning.Contract.Dtos.ReportScore
{
    public class CreateReportScoreCourseDto
    {
        public Guid Id { get; set; }

        public Guid QuizCourseId { get; set; }

        public Guid CourseId { get; set; }


        public string UserName { get; set; }
        public string FullName { get; set; }

        public DateTime CompletedDate { get; set; }

        public int TotalScore { get; set; }

        public bool Passed { get; set; }
        public string IpAddress { get; set; }

    }
}
