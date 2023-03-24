using FreeCourse.Services.FakePayment.DTOs;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Messages.Commands;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FreeCourse.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentsController : CustomBaseController
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public FakePaymentsController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePayment(PaymentVM data)
        {
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service")).ConfigureAwait(false);
            var request = new CreateOrderMessageCommands()
            {
                BuyerId = data.Order.BuyerId,
                District = data.Order.Address.District,
                Line = data.Order.Address.Line,
                Province = data.Order.Address.Province,
                Street = data.Order.Address.Street,
                ZipCode  = data.Order.Address.ZipCode
            };

            data.Order.OrderItems.ForEach(x =>
            {
                request.OrderItems.Add(new Shared.Messages.Commands.OrderItemDto()
                {
                    PictureUrl = x.PictureUrl,
                    Price = x.Price,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName
                });
            });

            await sendEndpoint.Send(request).ConfigureAwait(false);
            return CreateActionResultInstance(Shared.Dtos.Response<NoContent>.Success(200));
        }
    }
}
