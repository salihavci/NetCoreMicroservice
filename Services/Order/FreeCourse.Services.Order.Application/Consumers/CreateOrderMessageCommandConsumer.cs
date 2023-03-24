using FreeCourse.Services.Order.Infrastracture;
using FreeCourse.Shared.Messages.Commands;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Consumers
{
    public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommands>
    {
        private readonly OrderDbContext _dbContext;

        public CreateOrderMessageCommandConsumer(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<CreateOrderMessageCommands> context)
        {
            var newAddress = new Domain.OrderAggregate.Address(context.Message.Province, context.Message.District, context.Message.Street, context.Message.ZipCode, context.Message.Line);
            var order = new Domain.OrderAggregate.Order(newAddress, context.Message.BuyerId);
            context.Message.OrderItems.ForEach(x =>
            {
                order.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl);
            });
            await _dbContext.Orders.AddAsync(order).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
