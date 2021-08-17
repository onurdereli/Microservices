using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Course.Services.Basket.Dtos;
using Course.Services.Basket.Services.Abstract;
using Course.Shared.ControllerBase;
using Course.Shared.Services.Abstract;

namespace Course.Services.Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : CustomBaseController
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public BasketController(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            return CreateActionResultInstance(await _basketService.GetBasketAysnc(_sharedIdentityService.GetUserId));
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateBasket(BasketDto basketDto)
        {
            basketDto.UserId = _sharedIdentityService.GetUserId;

            return CreateActionResultInstance(await _basketService.SaveOrUpdateAysnc(basketDto));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket()
        {
            return CreateActionResultInstance(await _basketService.DeleteAysnc(_sharedIdentityService.GetUserId));
        }
    }
}
 