using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Course.Shared.Dtos;
using Course.Shared.Services.Abstract;
using Course.Web.Models.FakePayments;
using Course.Web.Models.Orders;
using Course.Web.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Course.Web.Services.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentService _paymentService;

        private readonly HttpClient _httpClient;

        private readonly IBasketService _basketService;

        private readonly ISharedIdentityService _sharedIdentityService;

        public OrderService(IBasketService basketService, HttpClient httpClient, IPaymentService paymentService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _httpClient = httpClient;
            _paymentService = paymentService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput)
        {
            var basket = await _basketService.Get();

            PaymentInfoInput paymentInfoInput = new()
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                Expiration = checkoutInfoInput.Expiration,
                Cvv = checkoutInfoInput.Cvv,
                TotalPrice = basket.TotalPrice
            };

            //var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);

            //if (!responsePayment)
            //{
            //    return new OrderCreatedViewModel { Error = "Ödeme alınamadı", IsSuccessfull = false };
            //}

            OrderCreateInput orderCreateInput = new()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new()
                {
                    Province = checkoutInfoInput.Province,
                    District = checkoutInfoInput.District,
                    Line = checkoutInfoInput.Line,
                    Street = checkoutInfoInput.Street,
                    ZipCode = checkoutInfoInput.ZipCode
                }
            };

            basket.BasketItems.ForEach(item =>
            {
                orderCreateInput.OrderItems.Add(new()
                {
                    Price = item.CurrentPrice,
                    ProductId = item.CourseId,
                    ProductName = item.CourseName
                });
            });

            var response = await _httpClient.PostAsJsonAsync<OrderCreateInput>("orders", orderCreateInput);
            if (!response.IsSuccessStatusCode)
            {
                return new OrderCreatedViewModel { Error = "Sipariş oluşturulamadı", IsSuccessfull = false };
            }

            var orderCreatedViewModel = await response.Content.ReadFromJsonAsync<Response<OrderCreatedViewModel>>();
            orderCreatedViewModel.Data.IsSuccessfull = true;
            await _basketService.Delete();
            return orderCreatedViewModel.Data;
        }

        public async Task<SuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInfoInput)
        {
            var basket = await _basketService.Get();

            OrderCreateInput orderCreateInput = new()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new()
                {
                    Province = checkoutInfoInput.Province,
                    District = checkoutInfoInput.District,
                    Line = checkoutInfoInput.Line,
                    Street = checkoutInfoInput.Street,
                    ZipCode = checkoutInfoInput.ZipCode
                }
            };

            basket.BasketItems.ForEach(item =>
            {
                orderCreateInput.OrderItems.Add(new()
                {
                    Price = item.CurrentPrice,
                    ProductId = item.CourseId,
                    ProductName = item.CourseName
                });
            });

            PaymentInfoInput paymentInfoInput = new()
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                Expiration = checkoutInfoInput.Expiration,
                Cvv = checkoutInfoInput.Cvv,
                TotalPrice = basket.TotalPrice,
                Order = orderCreateInput
            };

            var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);

            if (!responsePayment)
            {
                return new SuspendViewModel { Error = "Ödeme alınamadı", IsSuccessfull = false };
            }

            await _basketService.Delete();

            return new SuspendViewModel { IsSuccessfull = true };
        }

        public async Task<List<OrderViewModel>> GetOrder()
        {
            var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");

            return response?.Data;
        }
    }
}
