using WebLearning.Contract.Dtos.AnswerCourse;
using WebLearning.Contract.Dtos.CorrectAnswerCourse;
using WebLearning.Contract.Dtos.OptionCourse;
using WebLearning.Contract.Dtos.Quiz;

namespace WebLearning.Contract.Dtos.Question
{
    public class QuestionCourseDto
    {
        public Guid Id { get; set; }

        public Guid QuizCourseId { get; set; }

        public bool Active { get; set; }
        public string Name { get; set; }

        public int Point { get; set; }

        public string Alias { get; set; }

        public string Code { get; set; }
        public DateTime DateCreated { get; set; }
        public QuizCourseDto QuizCourseDto { get; set; }

        public virtual ICollection<AnswerCourseDto> AnswerCourseDtos { get; set; }

        public virtual ICollection<CorrectAnswerCourseDto> CorrectAnswerCourseDtos { get; set; }
        public virtual ICollection<OptionCourseDto> OptionCourseDtos { get; set; }
    }
}
