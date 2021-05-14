using System.Threading;
using System.Threading.Tasks;
using Course.Services.Order.Application.Commands;
using Course.Services.Order.Application.Dtos;
using Course.Services.Order.Domain.OrderAggregate;
using Course.Services.Order.Instrastructure;
using Course.Shared.Dtos;
using MediatR;

namespace Course.Services.Order.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
    {
        private readonly OrderDbContext _context;

        public CreateOrderCommandHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var newAddress = Mapping.ObjectMapping.Mapper.Map<Address>(request.Address);
            Domain.OrderAggregate.Order newOrder = new(request.BuyerId, newAddress);

            request.OrderItems.ForEach(orderItem =>
            {
                newOrder.AddOrderItem(orderItem.ProductId, orderItem.ProductName, orderItem.PictureUrl, orderItem.Price);
            });

            await _context.Orders.AddAsync(newOrder, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Response<CreatedOrderDto>.Success(new CreatedOrderDto { OrderId = newOrder.Id }, 200);
        }

    }
}
