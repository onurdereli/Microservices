using System;
using System.Collections.Generic;
using System.Text;
using Course.Shared.Messages.Commands.Concreate;

namespace Course.Shared.Messages.Commands.Abstract
{
    public interface ICreateOrderMessageCommand
    {
        public string BuyerId { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public Address Address { get; set; }
    }
}
