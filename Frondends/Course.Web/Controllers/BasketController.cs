using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Course.Web.Models.Baskets;
using Course.Web.Models.Discount;
using Course.Web.Services.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace Course.Web.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;

        public BasketController(ICatalogService catalogService, IBasketService basketService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _basketService.Get());
        }

        public async Task<IActionResult> AddBasketItem(string courseId)
        {
            var courseViewModel = await _catalogService.GetByCourseId(courseId);

            BasketItemViewModel basketItem = new()
            {
                CourseId = courseId,
                CourseName = courseViewModel.Name,
                Price = courseViewModel.Price,
                Quantity = 1
            };

            await _basketService.AddBasketItem(basketItem);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteBasketItem(string courseId)
        {
            await _basketService.RemoveBasketItem(courseId);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ApplyDiscount(DiscountApplyInput discountApplyInput)
        {
            // İndex'e yönlendirme olduğu için hata es geçiliyor bunun için tempdatada tutularak ekrana yansıtılır
            if(!ModelState.IsValid)
            {
                TempData["discountError"] = string.Join(" | ", ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage));
            }else
            {
                var discountStatus = await _basketService.ApplyDiscount(discountApplyInput.Code);

                TempData["discountStatus"] = discountStatus;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CancelAppliedDiscount()
        {
            await _basketService.CancelAppliedDiscount();

            return RedirectToAction(nameof(Index));
        }
    }
}
