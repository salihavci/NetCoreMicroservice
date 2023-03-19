using FreeCourse.Web.Abstractions;
using FreeCourse.Web.Inputs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _client;

        public PaymentService(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> ReceivePayment(PaymentInfoInput data)
        {
            var response = await _client.PostAsJsonAsync("fakepayments",data).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }
    }
}
