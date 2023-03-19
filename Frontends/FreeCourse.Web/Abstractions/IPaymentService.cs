using FreeCourse.Web.Inputs;
using System.Threading.Tasks;

namespace FreeCourse.Web.Abstractions
{
    public interface IPaymentService
    {
        Task<bool> ReceivePayment(PaymentInfoInput data);
    }
}
