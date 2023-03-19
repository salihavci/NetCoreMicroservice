using FluentValidation;
using FreeCourse.Web.Inputs;

namespace FreeCourse.Web.Validator
{
    public class CheckoutInfoValidator : AbstractValidator<CheckoutInfoInput>
    {
        public CheckoutInfoValidator()
        {
            RuleFor(x => x.Province).NotEmpty().WithMessage("İl alanı boş bırakılamaz.");
            RuleFor(x => x.District).NotEmpty().WithMessage("İlçe alanı boş bırakılamaz.");
            RuleFor(x => x.Street).NotEmpty().WithMessage("Cadde alanı boş bırakılamaz.");
            RuleFor(x => x.ZipCode).NotEmpty().WithMessage("Posta kodu alanı boş bırakılamaz.");
            RuleFor(x => x.Line).NotEmpty().WithMessage("Adres alanı boş bırakılamaz.");
            RuleFor(x => x.CardName).NotEmpty().WithMessage("Kart sahibinin adı - soyadı alanı boş bırakılamaz.");
            RuleFor(x => x.CardNumber).NotEmpty().WithMessage("Kart numarası alanı boş bırakılamaz.");
            RuleFor(x => x.Expiration).NotEmpty().WithMessage("Kart geçerlilik süresi alanı boş bırakılamaz.");
            RuleFor(x => x.CVV).NotEmpty().WithMessage("Kart CVV/CVC2 alanı boş bırakılamaz.");
        }
    }
}
