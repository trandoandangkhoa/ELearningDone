using FluentValidation;
using WebLearning.Contract.Dtos.Lession.LessionAdminView;

namespace WebLearning.Application.Validation
{
    public class CreateLessionValidation : AbstractValidator<CreateLessionAdminView>
    {
        public CreateLessionValidation()
        {
            RuleFor(ls => ls.CreateLessionDto.Name).NotEmpty().WithMessage("Vui lòng điền tên chương");
            RuleFor(ls => ls.CreateLessionDto.ShortDesc).NotEmpty().WithMessage("Vui lòng điền tóm tắt chương!");


        }
    }
}
