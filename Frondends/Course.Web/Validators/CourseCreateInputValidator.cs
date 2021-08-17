using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Course.Web.Models.Catalog;
using FluentValidation;

namespace Course.Web.Validators
{
    public class CourseCreateInputValidator: AbstractValidator<CourseCreateInput>
    {
        public CourseCreateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim alanı boş olamaz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama alanı boş olmaz");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("Süre alanı boş olamaz");
            //$$$$.$$ toplam 6lı sayıya sahip noktadan sonra 2 sayı alabilir
            RuleFor(x => x.Price).NotEmpty().WithMessage("Fiyat alanı boş olamaz.").ScalePrecision(2, 6);
        }
    }
}
