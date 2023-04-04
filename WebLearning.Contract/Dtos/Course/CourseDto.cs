using WebLearning.Contract.Dtos.CourseRole;
using WebLearning.Contract.Dtos.ImageCourse;
using WebLearning.Contract.Dtos.Lession;
using WebLearning.Contract.Dtos.Quiz;

namespace WebLearning.Contract.Dtos.Course
{
    public class CourseDto
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public bool Active { get; set; }
        //public Guid RoleId { get; set; }

        public DateTime DateCreated { get; set; }
        public string Alias { get; set; }

        //public RoleDto roleDto { get; set; }
        public bool Notify { get; set; }

        public string DescNotify { get; set; }

        public int TotalWatched { get; set; }
        public Guid CreatedBy { get; set; }

        public virtual QuizCourseDto QuizCourseDto { get; set; }

        public virtual ICollection<CourseImageDto> CourseImageVideoDto { get; set; }


        public ICollection<LessionDto> LessionDtos { get; set; }

        public ICollection<CourseRoleDto> CourseRoleDtos { get; set; }

    }
}
