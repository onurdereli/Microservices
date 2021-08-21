using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Models.Baskets
{
    public class BasketViewModel
    {
        public BasketViewModel()
        {
            _basketItems = new List<BasketItemViewModel>();
        }

        public string UserId { get; set; }

        public string DiscountCode { get; set; }

        public int? DiscountRate { get; set; }

        private List<BasketItemViewModel> _basketItems;

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

        public bool HasDiscount => !string.IsNullOrEmpty(DiscountCode) && DiscountRate.HasValue;

        public decimal TotalPrice => _basketItems.Sum(item => item.CurrentPrice * item.Quantity);

        public void CancelDiscount()
        {
            DiscountRate = null;
            DiscountCode = null;
        }

        public void ApplyDiscount(string code, int rate)
        {
            DiscountCode = code;
            DiscountRate = rate;
        }
    }
}
