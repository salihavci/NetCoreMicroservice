using FreeCourse.Services.Order.Application.Commands;
using FreeCourse.Services.Order.Application.Queries;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : CustomBaseController
    {
        private readonly IMediator _mediatr;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrdersController(IMediator mediatr, ISharedIdentityService sharedIdentityService)
        {
            _mediatr = mediatr;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var response = await _mediatr.Send(new GetOrdersByUserIdQuery() { UserId = _sharedIdentityService.GetUserId }).ConfigureAwait(false);
            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrder(CreateOrderCommand data)
        {
            var response = await _mediatr.Send(data).ConfigureAwait(false);
            return CreateActionResultInstance(response);
        }
    }
}
