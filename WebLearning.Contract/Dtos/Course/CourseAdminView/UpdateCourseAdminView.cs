using WebLearning.Contract.Dtos.ImageCourse;

namespace WebLearning.Contract.Dtos.Course.CourseAdminView
{
    public class UpdateCourseAdminView
    {
        public UpdateCourseDto UpdateCourseDto { get; set; } = new UpdateCourseDto();

        public UpdateCourseImageDto UpdateCourseImageDto { get; set; } = new UpdateCourseImageDto();

    }
}
