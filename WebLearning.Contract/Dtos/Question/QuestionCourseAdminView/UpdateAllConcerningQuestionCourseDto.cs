using WebLearning.Contract.Dtos.CorrectAnswerCourse;
using WebLearning.Contract.Dtos.OptionCourse;

namespace WebLearning.Contract.Dtos.Question.QuestionCourseAdminView
{
    public class UpdateAllConcerningQuestionCourseDto
    {
        public Guid[] OptionCourseId { get; set; }

        public Guid[] CorrectAnswerId { get; set; }

        public string[] OptionCourses { get; set; }

        public bool[] CorrectAnswers { get; set; }

        public string[] NewOptionCourses { get; set; }

        public bool[] NewCorrectAnswers { get; set; }

        public UpdateQuestionCourseDto UpdateQuestionCourse { get; set; }

        public UpdateCorrectAnswerCourseDto UpdateCorrectAnswerCourse { get; set; }

        public UpdateOptionCourseDto UpdateOptionCourse { get; set; }

        public OptionAndCorrectCourseDto OptionAndCorrectCourseDto { get; set; } = new OptionAndCorrectCourseDto();

        public List<NewOptionAndCorrectCourseDto> NewOptionAndCorrectCourseDtos { get; set; } = new List<NewOptionAndCorrectCourseDto>();

    }

    public class OptionAndCorrectCourseDto
    {
        public List<OptionCourseDto> OptionCourseDtos { get; set; } = new List<OptionCourseDto>();

        public List<CorrectAnswerCourseDto> CorrectAnswerCourseDtos { get; set; } = new List<CorrectAnswerCourseDto>();

    }
    public class NewOptionAndCorrectCourseDto
    {
        public string NewOptionCourses { get; set; }
        public bool NewCorrectAnswers { get; set; }
    }

}
