using System.Threading.Tasks;
using Course.Web.Models.Discount;

namespace Course.Web.Services.Abstract
{
    public interface IDiscountService
    {
        Task<DiscountViewModel> GetDiscount(string discountCode);
    }
}
