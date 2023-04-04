using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.Role;

namespace WebLearning.Contract.Dtos.CourseRole
{
    public class CourseRoleDto
    {
        public Guid Id { get; set; }

        public string Code { get; set; }


        public Guid CourseId { get; set; }

        public Guid RoleId { get; set; }

        public string RoleName { get; set; }

        public CourseDto CourseDto { get; set; }

        public RoleDto RoleDto { get; set; }

        public int NumWatched { get; set; }

    }
}
