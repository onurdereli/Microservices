using Course.Web.Models.Orders;

namespace Course.Web.Models.FakePayments
{
    public class PaymentInfoInput
    {
        public string CardName { get; set; }

        public string CardNumber { get; set; }

        public string Expiration { get; set; }

        public string Cvv { get; set; }

        public decimal TotalPrice { get; set; }

        public OrderCreateInput Order { get; set; }
    }
}
