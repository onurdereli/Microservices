﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.Web.Models.Discount;
using FluentValidation;

namespace Course.Web.Validators
{
    public class DiscountApplyInputValidator :AbstractValidator<DiscountApplyInput>
    {
        public DiscountApplyInputValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("İndirim kodu boş olamaz");
        }
    }
}
