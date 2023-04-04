using WebLearning.Contract.Dtos.Account;
using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.Lession;
using WebLearning.Contract.Dtos.Quiz;

namespace WebLearning.App.Models
{
    public class DashboardViewModel
    {
        public IEnumerable<CourseDto> CourseDtos { get; set; }

        public IEnumerable<AccountDto> AccountDtos { get; set; }
        public IEnumerable<LessionDto> LessionDtos { get; set; }

        public IEnumerable<QuizCourseDto> QuizCourseDtos { get; set; }
        public IEnumerable<QuizlessionDto> QuizlessionDtos { get; set; }
        public IEnumerable<QuizMonthlyDto> QuizMonthlyDtos { get; set; }

    }
}
