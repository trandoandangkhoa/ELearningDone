using FluentValidation;
using WebLearning.Contract.Dtos.Course.CourseAdminView;

namespace WebLearning.Application.Validation
{
    public class UpdateCourseValidation : AbstractValidator<UpdateCourseAdminView>
    {
        public UpdateCourseValidation()
        {
            // Check name is not null, empty and is between 1 and 250 characters
            RuleFor(course => course.UpdateCourseDto.Name).NotEmpty().WithMessage("Vui lòng điền tên khóa học!");
            RuleFor(course => course.UpdateCourseDto.Description).NotEmpty().WithMessage("Vui lòng điền tóm tắt khóa học!");
        }
    }
}
