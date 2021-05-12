using System.Text.Json;
using System.Threading.Tasks;
using Course.Services.Basket.Dtos;
using Course.Services.Basket.Services.Abstract;
using Course.Shared.Dtos;

namespace Course.Services.Basket.Services.Concrete
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task<Response<BasketDto>> GetBasketAysnc(string userId)
        {
            var existBasket = await _redisService.GetDb().StringGetAsync(userId);

            if (existBasket.IsNullOrEmpty)
            {
                return Response<BasketDto>.Fail("Basket not found", 404);
            }

            return Response<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(existBasket), 200);
        }

        public async Task<Response<bool>> SaveOrUpdateAysnc(BasketDto basketDto)
        {
            var status = await _redisService.GetDb().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));

            return status ? Response<bool>.Success(204) : Response<bool>.Fail("Basket could not update or save", 500);
        }

        public async Task<Response<bool>> DeleteAysnc(string userId)
        {
            var status = await _redisService.GetDb().KeyDeleteAsync(userId);

            return status ? Response<bool>.Success(204) : Response<bool>.Fail("Basket not found", 404);
        }
    }
}