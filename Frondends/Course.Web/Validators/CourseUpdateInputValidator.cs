using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.Web.Models.Catalogs;
using FluentValidation;

namespace Course.Web.Validators
{
    public class CourseUpdateInputValidator:AbstractValidator<CourseUpdateInput>
    {
        public CourseUpdateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim alanı boş olamaz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama alanı boş olmaz");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("Süre alanı boş olamaz");
            //$$$$.$$ toplam 6lı sayıya sahip noktadan sonra 2 sayı alabilir
            RuleFor(x => x.Price).NotEmpty().ScalePrecision(2, 6).WithMessage("Fiyat alanı boş olamaz.");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kategori alanı seçiniz");
        }
    }
}
