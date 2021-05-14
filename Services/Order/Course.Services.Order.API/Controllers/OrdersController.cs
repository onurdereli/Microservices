using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Course.Services.Order.Application.Commands;
using Course.Services.Order.Application.Queries;
using Course.Shared.ControllerBase;
using Course.Shared.Services.Abstract;
using MediatR;

namespace Course.Services.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : CustomBaseController
    {
        private readonly IMediator _mediator;

        private readonly ISharedIdentityService _sharedIdentityService;

        public OrdersController(IMediator mediator, ISharedIdentityService sharedIdentityService)
        {
            _mediator = mediator;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            return CreateActionResultInstance(await _mediator.Send(new GetOrdersByUserIdQuery { UserId = _sharedIdentityService.GetUserId }));
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrder(CreateOrderCommand createOrderCommand)
        {
            return CreateActionResultInstance(await _mediator.Send(createOrderCommand));
        }
    }
}
