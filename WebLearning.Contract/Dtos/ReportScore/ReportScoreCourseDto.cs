using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.Quiz;

namespace WebLearning.Contract.Dtos.ReportScore
{
    public class ReportScoreCourseDto
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

        public virtual ICollection<QuizCourseDto> QuizCourseDtos { get; set; }
        public virtual ICollection<CourseDto> CourseDtos { get; set; }
    }
}
