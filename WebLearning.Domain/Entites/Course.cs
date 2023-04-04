using System.ComponentModel.DataAnnotations;

namespace WebLearning.Domain.Entites
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; }

        public string Code { get; set; }



        public string Name { get; set; }


        public string Description { get; set; }


        public bool Active { get; set; }



        public DateTime DateCreated { get; set; }


        public string Alias { get; set; }


        public bool Notify { get; set; }

        public string DescNotify { get; set; }

        public int TotalWatched { get; set; }
        public Guid CreatedBy { get; set; }

        public virtual QuizCourse QuizCourse { get; set; }

        public virtual ICollection<CourseImageVideo> CourseImageVideo { get; set; }

        public virtual ICollection<Lession> Lessions { get; set; }

        public virtual ICollection<OtherFileUpload> OtherFileUploads { get; set; }

        public virtual ICollection<CourseRole> CourseRoles { get; set; }

    }
}
