using FluentValidation;
using WebLearning.Contract.Dtos.Question.QuestionMonthlyAdminView;

namespace WebLearning.Application.Validation
{
    public class UpdateQuestionMonthlyValidation : AbstractValidator<UpdateAllConcerningQuestionMonthlyDto>
    {
        public UpdateQuestionMonthlyValidation()
        {
            RuleFor(ls => ls.UpdateQuestionMonthly.Name).NotEmpty().WithMessage("Vui lòng điền nội dung câu hỏi!");
            RuleFor(ls => ls.UpdateQuestionMonthly.Point).InclusiveBetween(1, 1000).WithMessage("Điểm câu hỏi phải từ 1 - 1000")
                                                    .NotNull().NotEmpty().WithMessage("Vui lòng điền số điểm!");
            RuleFor(ls => ls.UpdateQuestionMonthly.Name).NotEmpty().WithMessage("Vui lòng điền lựa chọn câu hỏi!");
        }
    }
}
