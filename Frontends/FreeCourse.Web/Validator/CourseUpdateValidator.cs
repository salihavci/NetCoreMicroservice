using FluentValidation;
using FreeCourse.Web.Inputs;

namespace FreeCourse.Web.Validator
{
    public class CourseUpdateValidator : AbstractValidator<UpdateCourseInput>
    {
        public CourseUpdateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim alanı boş olamaz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama alanı boş olamaz");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("Süre alanı boş olamaz");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Fiyat alanı boş olamaz").ScalePrecision(2, 6).WithMessage("Hatalı fiyat formatı. Örn: 1234.56");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kategori alanı seçiniz");
        }
    }
}
