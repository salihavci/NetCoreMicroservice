using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Abstractions;
using FreeCourse.Web.DTOs.Baskets;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _client;
        private readonly IDiscountService _discountService;

        public BasketService(HttpClient client, IDiscountService discountService)
        {
            _client = client;
            _discountService = discountService;
        }

        public async Task AddBasketItem(BasketItemDto data)
        {
            var basket = await GetBasketAsync().ConfigureAwait(false);
            if (basket != null)
            {
                if (!basket.BasketItems.Any(x => x.CourseId == data.CourseId))
                {
                    basket.BasketItems.Add(data);
                }
            }
            else
            {
                basket = new BasketDto();
                basket.BasketItems.Add(data);
            }
            var saveBasketResponse = await SaveOrUpdateBasketAsync(basket);
        }

        public async Task<bool> ApplyDiscount(string discountCode)
        {
            var cancelDiscountResponse = await CancelDiscount().ConfigureAwait(false);
            var basket = await GetBasketAsync().ConfigureAwait(false);
            if(basket == null || string.IsNullOrWhiteSpace(discountCode))
            {
                return false;
            }
            var discount = await _discountService.GetDiscount(discountCode).ConfigureAwait(false);
            if(discount == null)
            {
                return false;
            }
            basket.ApplyDiscount(discount.Code, discount.Rate);
            var saveBasketResponse = await SaveOrUpdateBasketAsync(basket).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> CancelDiscount()
        {
            var basket = await GetBasketAsync().ConfigureAwait(false);
            if (basket == null || string.IsNullOrWhiteSpace(basket.DiscountCode))
            {
                return false;
            }
            basket.CancelDiscount();
            var saveBasketResponse = await SaveOrUpdateBasketAsync(basket).ConfigureAwait(false);
            return saveBasketResponse;
        }

        public async Task<bool> DeleteBasketAsync()
        {
            var response = await _client.DeleteAsync("baskets").ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }

        public async Task<BasketDto> GetBasketAsync()
        {
            var response = await _client.GetAsync("baskets").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var basketResponse = await response.Content.ReadFromJsonAsync<Response<BasketDto>>();
                return basketResponse.Data;
            }
            return null;
        }

        public async Task<bool> RemoveBasketItem(string courseId)
        {
            var basket = await GetBasketAsync().ConfigureAwait(false);
            if (basket == null)
            {
                return false;
            }

            var removedItem = basket.BasketItems.FirstOrDefault(x => x.CourseId == courseId);
            if (removedItem == null)
            {
                return false;
            }

            var deleteResult = basket.BasketItems.Remove(removedItem);
            if (!deleteResult)
            {
                return false;
            }

            if (!basket.BasketItems.Any())
            {
                basket.DiscountCode = null;
            }

            var basketResponse = await SaveOrUpdateBasketAsync(basket).ConfigureAwait(false);
            return basketResponse;
        }

        public async Task<bool> SaveOrUpdateBasketAsync(BasketDto data)
        {
            var response = await _client.PostAsJsonAsync<BasketDto>("baskets", data).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }
    }
}
