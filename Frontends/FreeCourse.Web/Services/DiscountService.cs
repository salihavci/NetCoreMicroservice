using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Abstractions;
using FreeCourse.Web.DTOs.Discounts;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _client;

        public DiscountService(HttpClient client)
        {
            _client = client;
        }

        public async Task<DiscountDto> GetDiscount(string discountCode)
        {
            //[controller]/[action]/{code}
            var response = await _client.GetAsync($"discounts/GetByCode/{discountCode}").ConfigureAwait(false);
            if(!response.IsSuccessStatusCode)
            {
                return null;
            }
            var discount = await response.Content.ReadFromJsonAsync<Response<DiscountDto>>().ConfigureAwait(false);
            return discount.Data;
        }
    }
}
