using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.LessionFileDocument;
using WebLearning.Contract.Dtos.Quiz;
using WebLearning.Contract.Dtos.ReportScore;
using WebLearning.Contract.Dtos.VideoLession;

namespace WebLearning.Contract.Dtos.Lession
{
    public class LessionDto
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("Course")]
        public Guid CourseId { get; set; }
        public string Code { get; set; }


        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public bool Active { get; set; }

        public DateTime DateCreated { get; set; }
        public string Alias { get; set; }
        public string Author { get; set; }

        public bool Notify { get; set; }

        public string DescNotify { get; set; }

        public virtual CourseDto CourseDto { get; set; }
        public virtual List<LessionVideoDto> LessionVideoDtos { get; set; }
        public virtual List<LessionFileDocumentDto> LessionFileDocumentDtos { get; set; }


        public virtual List<QuizlessionDto> QuizlessionDtos { get; set; }

        public virtual List<ReportScoreLessionDto> ReportScoreLessionDtos { get; set; }

        public virtual List<QuizCourseDto> QuizCourseDtos { get; set; }

        public virtual List<ReportScoreCourseDto> ReportScoreCourseDtos { get; set; }
    }
}
