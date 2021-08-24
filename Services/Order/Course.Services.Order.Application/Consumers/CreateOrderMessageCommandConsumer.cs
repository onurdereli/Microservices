using System.Threading.Tasks;
using Course.Services.Order.Instrastructure;
using Course.Shared.Messages.Commands.Abstract;
using MassTransit;

namespace Course.Services.Order.Application.Consumers
{
    public class CreateOrderMessageCommandConsumer:IConsumer<ICreateOrderMessageCommand>
    {
        private readonly OrderDbContext _orderDbContext;

        public CreateOrderMessageCommandConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<ICreateOrderMessageCommand> context)
        {
            var newAdress = new Domain.OrderAggregate.Address(context.Message.Address.Province, context.Message.Address.District, context.Message.Address.Street, context.Message.Address.ZipCode, context.Message.Address.Line);
            
            Domain.OrderAggregate.Order order = new Domain.OrderAggregate.Order(context.Message.BuyerId, newAdress);
            
            context.Message.OrderItems.ForEach(item=>
            {
                order.AddOrderItem(item.ProductId, item.ProductName, item.PictureUrl, item.Price);
            });

            await _orderDbContext.Orders.AddAsync(order);

            await _orderDbContext.SaveChangesAsync();
        }
    }
}
