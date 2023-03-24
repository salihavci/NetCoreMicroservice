using FreeCourse.Services.Basket.DTOs;
using FreeCourse.Services.Basket.Services;
using FreeCourse.Shared.Messages.Events;
using MassTransit;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Consumers
{
    public class CourseNameChangedEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly RedisService _redisService;

        public CourseNameChangedEventConsumer(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            var keys = _redisService.GetKeys();
            if(keys != null)
            {
                foreach(var key in keys)
                {
                    var basket = await _redisService.GetDb().StringGetAsync(key);
                    var basketDto = JsonSerializer.Deserialize<BasketVM>(basket);
                    basketDto.BasketItems.ForEach(x =>
                    {
                        x.CourseName = x.CourseId == context.Message.CourseId ? context.Message.UpdatedName : x.CourseName;
                    });
                    await _redisService.GetDb().StringSetAsync(key, JsonSerializer.Serialize(basketDto));
                }
            }
        }
    }
}
