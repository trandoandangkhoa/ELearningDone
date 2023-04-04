using System.ComponentModel.DataAnnotations;

namespace WebLearning.Domain.Entites
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Code { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }
        public string Alias { get; set; }

        public ICollection<Account> Accounts { get; set; }

        //public ICollection<Course> Courses { get; set; }
        public ICollection<CourseRole> CourseRoles { get; set; }


    }
}
