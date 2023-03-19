using FluentValidation;
using FreeCourse.Web.Inputs;

namespace FreeCourse.Web.Validator
{
    public class DiscountApplyValidator : AbstractValidator<DiscountApplyInput>
    {
        public DiscountApplyValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("İndirim kupon alanı boş bırakılamaz.");
        }
    }
}
