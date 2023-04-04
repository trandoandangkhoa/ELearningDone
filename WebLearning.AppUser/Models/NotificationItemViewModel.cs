using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.Notification;
using WebLearning.Contract.Dtos.Quiz;

namespace WebLearning.AppUser.Models
{
    public class NotificationItemViewModel
    {
        public List<NotificationResponseDto> NotificationResponseDtos { get; set; }

        public List<CourseDto> OwnCourseDtos { get; set; }


        public List<QuizMonthlyDto> QuizMonthlyDtos { get; set; }

    }
}
