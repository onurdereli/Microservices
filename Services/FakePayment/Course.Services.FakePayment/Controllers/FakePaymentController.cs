using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Course.Services.FakePayment.Models;
using Course.Shared.ControllerBase;
using Course.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Course.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FakePaymentController : CustomBaseController
    {
        [HttpPost]
        public IActionResult ReceivePayment()
        {
            // paymentDto işlemleri yapılabilir
            return CreateActionResultInstance(Response<NoContent>.Success(200));
        }
    }
}
