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
            var createdOrder = await _orderService.CreateOrder(checkoutInfoInput);

            if (!createdOrder.IsSuccessfull)
            {
                var basketViewModel = await _basketService.Get();

                ViewBag.basket = basketViewModel;
                ViewBag.error = createdOrder.Error;

                return View();
            }

            return RedirectToAction(nameof(SuccessfullCheckout), new { orderId = createdOrder.OrderId });
        }

        public IActionResult SuccessfullCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }
    }
}
