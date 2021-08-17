using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Models.Baskets
{
    public class BasketViewModel
    {
        public string UserId { get; set; }

        public string DiscountCode { get; set; }

        public int? DiscountRate { get; set; }

        private List<BasketItemViewModel> _basketItems { get; set; }

        public List<BasketItemViewModel> BasketItems
        {
            get
            {
                if (HasDiscount)
                {
                    _basketItems.ForEach(item =>
                    {
                        if (DiscountRate == null) return;
                        var discountPrice = item.Price * ((decimal)DiscountRate.Value / 100);
                        item.AppliedDiscount(Math.Round(item.Price - discountPrice, 2));
                    });
                }
                return _basketItems;
            }
            set => _basketItems = value;
        }

        public bool HasDiscount => !string.IsNullOrEmpty(DiscountCode);

        public decimal TotalPrice => _basketItems.Sum(item => item.Price * item.Quantity);
    }
}
