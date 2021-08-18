using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Models.Baskets
{
    public class BasketItemViewModel
    {
        public int Quantity { get; set; }

        public string CourseId { get; set; }

        public string CourseName { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountAppliedPrice;

        public decimal CurrentPrice => DiscountAppliedPrice ?? Price;

        public void AppliedDiscount(decimal discountPrice)
        {
            DiscountAppliedPrice = discountPrice;
        }
    }
}
