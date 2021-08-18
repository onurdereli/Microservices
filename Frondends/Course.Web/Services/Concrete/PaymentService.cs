﻿using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Course.Web.Models.FakePayments;
using Course.Web.Services.Abstract;

namespace Course.Web.Services.Concrete
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ReceivePayment(PaymentInfoInput paymentInfoInput)
        {
            var response = await _httpClient.PostAsJsonAsync<PaymentInfoInput>("fakepayment", paymentInfoInput);

            return response.IsSuccessStatusCode;
        }
    }
}
