using FreeCourse.Services.Basket.DTOs;
using FreeCourse.Shared.Dtos;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task<Response<bool>> Delete(string userId)
        {
            var status = await _redisService.GetDb().KeyDeleteAsync(userId).ConfigureAwait(false);
            return status ? Response<bool>.Success(204) : Response<bool>.Fail("Basket not found.", 404);
        }

        public async Task<Response<BasketVM>> GetBasket(string userId)
        {
            var existBasket = await _redisService.GetDb().StringGetAsync(userId).ConfigureAwait(false);
            if(string.IsNullOrEmpty(existBasket)) {
                return Response<BasketVM>.Fail("Basket not found.", 404);
            }
            return Response<BasketVM>.Success(JsonSerializer.Deserialize<BasketVM>(existBasket),200);
        }

        public async Task<Response<bool>> SaveOrUpdate(BasketVM data)
        {
            var status = await _redisService.GetDb().StringSetAsync(data.UserId, JsonSerializer.Serialize(data)).ConfigureAwait(false);
            return status ? Response<bool>.Success(204) : Response<bool>.Fail("Basket could not save or update.", 500);
        }
    }
}
