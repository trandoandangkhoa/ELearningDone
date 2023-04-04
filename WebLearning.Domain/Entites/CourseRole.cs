using System.ComponentModel.DataAnnotations;

namespace WebLearning.Domain.Entites
{
    public class CourseRole
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CourseId { get; set; }

        public Guid RoleId { get; set; }

        public string Code { get; set; }


        public string RoleName { get; set; }

        public Course Course { get; set; }

        public Role Role { get; set; }

        public int NumWatched { get; set; }

    }
}
