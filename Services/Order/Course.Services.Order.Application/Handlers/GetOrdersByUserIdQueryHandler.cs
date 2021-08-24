﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Course.Services.Order.Application.Configuration.Mappings;
using Course.Services.Order.Application.Dtos;
using Course.Services.Order.Application.Queries;
using Course.Services.Order.Instrastructure;
using Course.Shared.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Course.Services.Order.Application.Handlers
{
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, Response<List<OrderDto>>>
    {
        private readonly OrderDbContext _context;

        public GetOrdersByUserIdQueryHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders.Include(x => x.OrderItems).Where(x => x.BuyerId == request.UserId).ToListAsync();

            if (!orders.Any())
            {
                return Response<List<OrderDto>>.Success(new List<OrderDto>(), 200);
            }

            var ordersDto = ObjectMapper.Mapper.Map<List<OrderDto>>(orders);

            return Response<List<OrderDto>>.Success(ordersDto, 200);
        }
    }
}
