﻿using MediatR;
using System.Collections.Generic;
using Course.Services.Order.Application.Dtos;
using Course.Shared.Dtos;

namespace Course.Services.Order.Application.Commands
{
    public class CreateOrderCommand: IRequest<Response<CreatedOrderDto>>
    {
        public string BuyerId { get; set; }

        public List<OrderItemDto> OrderItems { get; set; }

        public AddressDto Address { get; set; }
    }
}
