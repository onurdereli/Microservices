using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.Services.FakePayment.Models;
using Course.Shared.ControllerBase;
using Course.Shared.Dtos;

namespace Course.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentController : CustomBaseController
    {
        [HttpGet]
        public IActionResult ReceivePayment(PaymentDto paymentDto)
        {
            // paymentDto işlemleri yapılabilir
            return CreateActionResultInstance(Response<NoContent>.Success(200));
        }
    }
}
