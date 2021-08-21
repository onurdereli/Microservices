using Course.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using Course.Web.Exceptions;
using Course.Web.Services.Abstract;
using Microsoft.AspNetCore.Diagnostics;

namespace Course.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ICatalogService _catalogService;

        public HomeController(ILogger<HomeController> logger, ICatalogService catalogService)
        {
            _logger = logger;
            _catalogService = catalogService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _catalogService.GetAllCourse());
        }

        public async Task<IActionResult> Detail(string id)
        {
            return View(await _catalogService.GetByCourseId(id));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // UnAuthorizeException hatası geldiğinde kullanıcıya logout yaptırmayı sağlar. Bunun amacı accesstoken süresi bittiğinde error almamak için.
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if(exceptionHandlerFeature is { Error: UnAuthorizeException })
            {
                return RedirectToAction(nameof(AuthController.Logout), "Auth");
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
