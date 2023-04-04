using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class ReportUserScoreFinal
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("QuizCourse")]
        public Guid QuizCourseId { get; set; }

        public Guid CourseId { get; set; }


        public string UserName { get; set; }
        public string FullName { get; set; }

        public DateTime CompletedDate { get; set; }

        public int TotalScore { get; set; }

        public bool Passed { get; set; }
        public string IpAddress { get; set; }

        public virtual ICollection<QuizCourse> QuizCourses { get; set; }
    }
}
