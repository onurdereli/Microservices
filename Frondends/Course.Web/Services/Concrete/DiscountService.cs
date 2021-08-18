using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Course.Shared.Dtos;
using Course.Web.Models.Discount;
using Course.Web.Services.Abstract;

namespace Course.Web.Services.Concrete
{
    public class DiscountService: IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DiscountViewModel> GetDiscount(string discountCode)
        {
            //[controller]/[action]/{code}

            var response = await _httpClient.GetAsync($"discount/GetByCode/{discountCode}");

            if (!response.IsSuccessStatusCode)
                return null;

            var discount = await response.Content.ReadFromJsonAsync<Response<DiscountViewModel>>();

            return discount?.Data;
        }
    }
}
