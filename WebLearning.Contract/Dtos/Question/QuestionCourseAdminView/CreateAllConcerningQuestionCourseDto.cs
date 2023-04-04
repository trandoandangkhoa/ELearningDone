using WebLearning.Contract.Dtos.CorrectAnswerCourse;
using WebLearning.Contract.Dtos.OptionCourse;

namespace WebLearning.Contract.Dtos.Question.QuestionCourseAdminView
{
    public class CreateAllConcerningQuestionCourseDto
    {

        public CreateQuestionCourseDto CreateQuestionCourseDto { get; set; }

        public CreateOptionCourseDto CreateOptionCourseDto { get; set; } = new CreateOptionCourseDto();

        public CreateCorrectAnswerCourseDto CreateCorrectAnswerCourseDto { get; set; } = new CreateCorrectAnswerCourseDto();

        public string[] OptionCourses { get; set; }

        public bool[] CorrectAnswers { get; set; }

        public List<OptionAndCorrectCourse> OptionAndCorrectCourses { get; set; } = new List<OptionAndCorrectCourse>();

        public QuestionCourseDto QuestionCourseDto { get; set; }
    }

    public class OptionAndCorrectCourse
    {
        public string OptionCourses { get; set; }
        public bool CorrectAnswers { get; set; }
    }


}
