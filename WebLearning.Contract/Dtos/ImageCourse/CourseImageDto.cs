using WebLearning.Contract.Dtos.Course;

namespace WebLearning.Contract.Dtos.ImageCourse
{
    public class CourseImageDto
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Caption { get; set; }
        public string ImagePath { get; set; }
        public DateTime DateCreated { get; set; }
        public long FileSize { get; set; }

        public CourseDto CourseDto { get; set; }
    }
}
