using System.Threading.Tasks;
using Course.Services.Basket.Dtos;
using Course.Shared.Dtos;

namespace Course.Services.Basket.Services.Abstract
{
    public interface IBasketService
    {
        Task<Response<BasketDto>> GetBasketAysnc(string userId);
        Task<Response<bool>> SaveOrUpdateAysnc(BasketDto basketDto);
        Task<Response<bool>> DeleteAysnc(string userId);
    }
}
