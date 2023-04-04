using WebLearning.Contract.Dtos.AnswerCourse;
using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.HistorySubmit;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.ReportScore;

namespace WebLearning.Contract.Dtos.Quiz
{
    public class QuizCourseDto
    {

        public Guid ID { get; set; }

        public Guid CourseId { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public DateTime DateCreated { get; set; }

        public int TimeToDo { get; set; }

        public int ScorePass { get; set; }

        public bool Passed { get; set; }

        public string Alias { get; set; }
        public bool Notify { get; set; }

        public string DescNotify { get; set; }


        public virtual CourseDto CourseDto { get; set; }

        public virtual ICollection<QuestionCourseDto> QuestionCourseDtos { get; set; }

        public virtual ICollection<HistorySubmitCourseDto> HistorySubmitCourseDtos { get; set; }

        public virtual ICollection<AnswerCourseDto> AnswerCourseDtos { get; set; }

        public virtual ICollection<ReportScoreCourseDto> ReportScoreCourseDtos { get; set; }
    }
}
