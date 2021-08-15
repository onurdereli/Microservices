using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.Shared.Services.Abstract;
using Course.Web.Models.Catalog;
using Course.Web.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Course.Web.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICatalogService _catalogService;

        private readonly ISharedIdentityService _sharedIdentityService;

        public CourseController(ISharedIdentityService sharedIdentityService, ICatalogService catalogService)
        {
            _sharedIdentityService = sharedIdentityService;
            _catalogService = catalogService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _catalogService.GetAllCourseByUserId(_sharedIdentityService.GetUserId));
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _catalogService.GetAllGategory();

            ViewBag.categoryList = new SelectList(categories, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateInput courseCreateInput)
        {
            var categories = await _catalogService.GetAllGategory();

            ViewBag.categoryList = new SelectList(categories, "Id", "Name");

            if (!ModelState.IsValid)
            {
                return View();
            }

            courseCreateInput.UserId = _sharedIdentityService.GetUserId;

            var isSuccess = await _catalogService.CreateCourse(courseCreateInput);

            if (!isSuccess)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(string id)
        {
            var course = await _catalogService.GetByCourseId(id);
            var categories = await _catalogService.GetAllGategory();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", course.CategoryId);


            CourseUpdateInput courseUpdateInput = new()
            {
                Id = course.Id,
                CategoryId = course.CategoryId,
                Description = course.Description,
                Feature = course.Feature,
                Name = course.Name,
                Picture = course.Picture,
                Price = course.Price,
                UserId = course.UserId
            };

            return View(courseUpdateInput);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateInput courseUpdateInput)
        {
            var categories = await _catalogService.GetAllGategory();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", courseUpdateInput.CategoryId);

            if(!ModelState.IsValid)
            {
                return View();
            }

            await _catalogService.UpdateCourse(courseUpdateInput);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _catalogService.DeleteCourse(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
