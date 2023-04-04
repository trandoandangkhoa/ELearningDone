using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class Lession
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

        public virtual Course Courses { get; set; }

        public virtual ICollection<LessionVideoImage> LessionVideoImages { get; set; }

        public virtual ICollection<OtherFileUpload> OtherFileUploads { get; set; }

        public virtual ICollection<QuizLession> Quizzes { get; set; }


    }
}
