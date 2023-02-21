using FreeCourse.Services.Discount.Services;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : CustomBaseController
    {
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IDiscountService _discountService;

        public DiscountsController(ISharedIdentityService sharedIdentityService, IDiscountService discountService)
        {
            _sharedIdentityService = sharedIdentityService;
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResultInstance(await _discountService.GetListAsync().ConfigureAwait(false));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var discount = await _discountService.GetByIdAsync(id).ConfigureAwait(false);
            return CreateActionResultInstance(discount);
        }

        [Route("/api/[controller]/[action]/{code}")]
        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            var userId = _sharedIdentityService.GetUserId;
            var discount = await _discountService.GetByCodeAndUserIdAsync(code, userId).ConfigureAwait(false);
            return CreateActionResultInstance(discount);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Models.Discount data)
        {
            return CreateActionResultInstance(await _discountService.SaveAsync(data).ConfigureAwait(false));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Models.Discount data)
        {
            return CreateActionResultInstance(await _discountService.UpdateAsync(data).ConfigureAwait(false));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActionResultInstance(await _discountService.DeleteAsync(id).ConfigureAwait(false));
        }
    }
}
