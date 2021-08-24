using System.Threading.Tasks;
using Course.Web.Models.FakePayments;

namespace Course.Web.Services.Abstract
{
    public interface IPaymentService
    {
        Task<bool> ReceivePayment(PaymentInfoInput paymentInfoInput);
    }
}
