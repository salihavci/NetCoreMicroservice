using FreeCourse.Services.Basket.DTOs;
using FreeCourse.Services.Basket.Services;
using FreeCourse.Shared.Messages.Events;
using MassTransit;
using System;
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
                    Console.WriteLine("Key : " + key.ToString());
                    var basket = await _redisService.GetDb().StringGetAsync(key);
                    var basketDto = JsonSerializer.Deserialize<BasketVM>(basket);
                    basketDto.BasketItems.ForEach(x =>
                    {
                        Console.WriteLine(context.Message.CourseId.ToString());
                        x.CourseName = x.CourseId == context.Message.CourseId ? context.Message.UpdatedName : x.CourseName;
                    });
                    await _redisService.GetDb().StringSetAsync(key, JsonSerializer.Serialize(basketDto));
                }
            }
        }
    }
}
