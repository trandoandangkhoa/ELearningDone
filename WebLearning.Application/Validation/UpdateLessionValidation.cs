using FluentValidation;
using WebLearning.Contract.Dtos.Lession;

namespace WebLearning.Application.Validation
{
    public class UpdateLessionValidation : AbstractValidator<UpdateLessionDto>
    {
        public UpdateLessionValidation()
        {
            RuleFor(ls => ls.Name).NotEmpty().WithMessage("Vui lòng điền tên chương");
            RuleFor(ls => ls.ShortDesc).NotNull().NotEmpty().WithMessage("Vui lòng điền tóm tắt chương!");
        }
    }
}
