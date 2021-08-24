using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Course.Web.Models.Orders;
using Course.Web.Services.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace Course.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;

        private readonly IOrderService _orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Checkout()
        {
            var basket = await _basketService.Get();
            ViewBag.basket = basket;

            return View(new CheckoutInfoInput());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutInfoInput checkoutInfoInput)
        {
            #region 1. yol senkron iletişim

            /*
             var orderStatus = await _orderService.CreateOrder(checkoutInfoInput);
            if (!orderStatus.IsSuccessfull)
            {
                var basketViewModel = await _basketService.Get();

                ViewBag.basket = basketViewModel;
                ViewBag.error = orderStatus.Error;

                return View();
            }
            return RedirectToAction(nameof(SuccessfullCheckout), new { orderId = orderStatus.OrderId });
             */

            #endregion

            //2. yol asenkron iletişim
            var orderSuspend = await _orderService.SuspendOrder(checkoutInfoInput);
            if (!orderSuspend.IsSuccessfull)
            {
                var basketViewModel = await _basketService.Get();

                ViewBag.basket = basketViewModel;
                ViewBag.error = orderSuspend.Error;

                return View();
            }
            //Senkron iletişimde id dönülmediği için hatayı engellemek için random bi değer atandı
            return RedirectToAction(nameof(SuccessfullCheckout), new { orderId = new Random().Next(1, 1000) });
        }

        public IActionResult SuccessfullCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }
        
        public async Task<IActionResult> CheckoutHistory()
        {
            return View(await _orderService.GetOrder());
        }
    }
}
