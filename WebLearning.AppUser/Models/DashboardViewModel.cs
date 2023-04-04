using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Account;
using WebLearning.Contract.Dtos.Avatar;
using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.Lession;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.Quiz;
using WebLearning.Contract.Dtos.ReportScore;
using WebLearning.Contract.Dtos.Role;

namespace WebLearning.AppUser.Models
{
    public class DashboardViewModel
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordHased { get; set; }

        public int Active { get; set; }

        public Guid RoleId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime LastLogin { get; set; }

        public AccountDetailDto AccountDetailDto { get; set; }

        public AvatarDto Avatar { get; set; }

        public RoleDto RoleDto { get; set; }

        public AuthorizeRole AuthorizeRole { get; set; }

        public List<CourseDto> CourseDtos { get; set; }

        public List<CourseDto> OwnCourseDtos { get; set; }

        public List<LessionDto> LessionDtos { get; set; }

        public List<QuizlessionDto> QuizlessionDtos { get; set; }

        public List<QuizCourseDto> QuizCourseDtos { get; set; }

        public List<QuizMonthlyDto> QuizMonthlyDtos { get; set; }

        public List<QuestionLessionDto> QuestionLessionDtos { get; set; }

        public List<ReportScoreLessionDto> ReportScoreLessionDtos { get; set; }

        public List<ReportScoreCourseDto> ReportScoreCourseDtos { get; set; }

        public List<ReportScoreMonthlyDto> ReportScoreMonthlyDtos { get; set; }




    }
}
