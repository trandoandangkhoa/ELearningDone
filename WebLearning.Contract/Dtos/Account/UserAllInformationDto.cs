using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.Lession;
using WebLearning.Contract.Dtos.Quiz;
using WebLearning.Contract.Dtos.ReportScore;

namespace WebLearning.Contract.Dtos.Account
{
    public class UserAllInformationDto
    {
        public AccountDto AccountDto { get; set; }

        public List<CourseDto> CourseDtos { get; set; }

        public List<CourseDto> OwnCourseDtos { get; set; }

        public List<LessionDto> LessionDtos { get; set; }

        public List<QuizlessionDto> QuizlessionDtos { get; set; }

        public List<QuizCourseDto> QuizCourseDtos { get; set; }

        public List<QuizMonthlyDto> QuizMonthlyDtos { get; set; }

        public List<ReportScoreLessionDto> ReportScoreLessionDtos { get; set; }

        public List<ReportScoreCourseDto> ReportScoreCourseDtos { get; set; }

        public List<ReportScoreMonthlyDto> ReportScoreMonthlyDtos { get; set; }
    }
}
