using FluentValidation;
using WebLearning.Contract.Dtos.Question.QuestionCourseAdminView;

namespace WebLearning.Application.Validation
{
    public class UpdateQuestionCourseValidation : AbstractValidator<UpdateAllConcerningQuestionCourseDto>
    {
        public UpdateQuestionCourseValidation()
        {
            RuleFor(ls => ls.UpdateQuestionCourse.Name).NotEmpty().WithMessage("Vui lòng điền nội dung câu hỏi cuối khóa!");
            RuleFor(ls => ls.UpdateQuestionCourse.Point).InclusiveBetween(1, 1000).WithMessage("Điểm câu hỏi phải từ 1 - 1000")
                                                    .NotNull().NotEmpty().WithMessage("Vui lòng điền số điểm!");
            RuleFor(ls => ls.UpdateQuestionCourse.Name).NotEmpty().WithMessage("Vui lòng điền lựa chọn câu hỏi!");
        }
    }
}
