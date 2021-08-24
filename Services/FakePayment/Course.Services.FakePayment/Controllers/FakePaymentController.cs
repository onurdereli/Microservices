
using System;
using System.Threading.Tasks;
using Course.Services.FakePayment.Models;
using Course.Shared.ControllerBase;
using Course.Shared.Dtos;
using Course.Shared.Messages;
using Course.Shared.Messages.Commands;
using Course.Shared.Messages.Commands.Concreate;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Course.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentController : CustomBaseController
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public FakePaymentController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePayment(PaymentDto paymentDto)
        {
            //Ödeme işleminde rabbitmqde bi queue oluşturup command'ı gönderilir
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));

            var createOrderMessageCommand = new CreateOrderMessageCommand()
            {
                Address = new()
                {
                    District = paymentDto.Order.Address.District,
                    Line = paymentDto.Order.Address.Line,
                    Province = paymentDto.Order.Address.Province,
                    Street = paymentDto.Order.Address.Street,
                    ZipCode = paymentDto.Order.Address.ZipCode
                },
                BuyerId = paymentDto.Order.BuyerId
            };
            paymentDto.Order.OrderItems.ForEach(item =>
            {
                createOrderMessageCommand.OrderItems.Add(new()
                {
                    PictureUrl = item.PictureUrl,
                    Price = item.Price,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName
                });
            });

            await sendEndpoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);

            return CreateActionResultInstance(Shared.Dtos.Response<NoContent>.Success(200));
        }
    }
}
