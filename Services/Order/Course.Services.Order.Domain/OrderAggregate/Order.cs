using System;
using System.Collections.Generic;
using System.Linq;
using Course.Services.Order.Domain.Core;

namespace Course.Services.Order.Domain.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        public DateTime CreatedDate { get; private set; }

        public Address Address { get; private set; }

        public string BuyerId { get; private set; }

        //Backing Field'dir - amaç; encapsulate arttırmak
        private readonly List<OrderItem> _orderItems;

        //EF Core datayı getirirken OrderItemlar sadece okunabilsin diye bu şekilde eklendi
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Order()
        {
            
        }

        public Order(string buyerId, Address address)
        {
            _orderItems = new List<OrderItem>();
            CreatedDate = DateTime.Now;
            Address = address;
            BuyerId = buyerId;
        }

        public void AddOrderItem(string productId, string productName, string pictureUrl, decimal price)
        {
            var existProduct = _orderItems.Any(item => item.ProductId == productId);

            if (!existProduct)
            {
                OrderItem orderItem = new(productId, productName, pictureUrl, price);

                _orderItems.Add(orderItem); ;
            }
        }

        public decimal GetTotalPrice => _orderItems.Sum(item => item.Price);
    }
}

//NOT: Domain hangi ortamda hangi ORM aracıyla çalıştığını bilmemeli herhangi bir kütüphaneye bağlılığı olmamalı