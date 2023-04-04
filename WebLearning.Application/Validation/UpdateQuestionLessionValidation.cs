using FluentValidation;
using WebLearning.Contract.Dtos.Question.QuestionLessionAdminView;

namespace WebLearning.Application.Validation
{
    public class UpdateQuestionLessionValidation : AbstractValidator<UpdateAllConcerningQuestionLesstionDto>
    {
        public UpdateQuestionLessionValidation()
        {
            RuleFor(ls => ls.UpdateQuestionLession.Name).NotEmpty().WithMessage("Vui lòng điền nội dung câu hỏi bài học!");
            RuleFor(ls => ls.UpdateQuestionLession.Point).InclusiveBetween(1, 1000).WithMessage("Điểm câu hỏi phải từ 1 - 1000")
                                                    .NotNull().NotEmpty().WithMessage("Vui lòng điền số điểm!");
            RuleFor(ls => ls.UpdateQuestionLession.Name).NotEmpty().WithMessage("Vui lòng điền lựa chọn câu hỏi!");
        }
    }
}
