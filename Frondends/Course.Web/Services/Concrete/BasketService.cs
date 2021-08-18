using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Course.Shared.Dtos;
using Course.Web.Models.Baskets;
using Course.Web.Services.Abstract;

namespace Course.Web.Services.Concrete
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;
        private readonly IDiscountService _discountService;

        public BasketService(HttpClient httpClient, IDiscountService discountService)
        {
            _httpClient = httpClient;
            _discountService = discountService;
        }


        public async Task<bool> SaveOrUpdate(BasketViewModel basketViewModel)
        {
            var response = await _httpClient.PostAsJsonAsync<BasketViewModel>("basket", basketViewModel);

            return response.IsSuccessStatusCode;
        }

        public async Task<BasketViewModel> Get()
        {
            var response = await _httpClient.GetAsync("basket");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var basketViewModelResponse = await response.Content.ReadFromJsonAsync<Response<BasketViewModel>>();

            return basketViewModelResponse?.Data;
        }

        public async Task<bool> Delete()
        {
            var resultResponse = await _httpClient.DeleteAsync("basket");

            return resultResponse.IsSuccessStatusCode;
        }

        public async Task AddBasketItem(BasketItemViewModel basketItemViewModel)
        {
            var basket = await Get();

            if (basket != null)
            {
                if (basket.BasketItems.All(item => item.CourseId != basketItemViewModel.CourseId))
                {
                    basket.BasketItems.Add(basketItemViewModel);
                }
            }
            else
            {
                basket = new BasketViewModel();
                basket.BasketItems.Add(basketItemViewModel);
            }

            await SaveOrUpdate(basket);
        }

        public async Task<bool> RemoveBasketItem(string courseId)
        {
            var basket = await Get();

            var deleteBasketItem = basket?.BasketItems.FirstOrDefault(item => item.CourseId == courseId);

            if (deleteBasketItem == null)
            {
                return false;
            }

            var isDeleteBasket = basket.BasketItems.Remove(deleteBasketItem);

            if (!isDeleteBasket)
            {
                return false;
            }

            if (!basket.BasketItems.Any())
            {
                basket.DiscountCode = null;
            }

            return await SaveOrUpdate(basket);
        }

        public async Task<bool> ApplyDiscount(string discountCode)
        {
            await CancelAppliedDiscount();

            var basket = await Get();

            if (basket == null)
            {
                return false;
            }

            var discount = await _discountService.GetDiscount(discountCode);

            if (discount == null)
            {
                return false;
            }

            basket.ApplyDiscount(discount.Code, discount.Rate);

            return await SaveOrUpdate(basket);
        }

        public async Task<bool> CancelAppliedDiscount()
        {
            var basket = await Get();

            if (basket?.DiscountCode == null)
            {
                return false;
            }
            
            basket.CancelDiscount();

            return await SaveOrUpdate(basket);
        }
    }
}
