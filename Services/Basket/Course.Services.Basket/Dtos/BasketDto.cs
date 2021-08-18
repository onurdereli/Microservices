using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Basket.Dtos
{
    public class BasketDto
    {
        public string UserId { get; set; }

        public string DiscountCode { get; set; }

        public int? DiscountRate { get; set; }

        public List<BasketItemDto> BasketItems { get; set; }

        public decimal TotalPrice => BasketItems.Sum(item => item.Price * item.Quantity);
    }
}
