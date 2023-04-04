using WebLearning.Contract.Dtos.CorrectAnswerCourse;
using WebLearning.Contract.Dtos.OptionCourse;
using WebLearning.Contract.Dtos.Question;

namespace WebLearning.App.Models
{
    public class CreateQuestionCourseViewModel
    {
        public CreateQuestionCourseDto CreateQuestionCourseDto { get; set; }
        public CreateOptionCourseDto CreateOptionCourseDto { get; set; }

        public CreateCorrectAnswerCourseDto CreateCorrectAnswerCourseDto { get; set; }
    }
}
