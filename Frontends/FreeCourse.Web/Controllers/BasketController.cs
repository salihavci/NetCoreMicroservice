using FreeCourse.Web.Abstractions;
using FreeCourse.Web.DTOs.Baskets;
using FreeCourse.Web.Inputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Web.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;

        public BasketController(ICatalogService catalogService, IBasketService basketService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
        }

        public async Task<IActionResult> Index()
        {
            var basket = await _basketService.GetBasketAsync().ConfigureAwait(false);
            return View(basket);
        }

        public async Task<IActionResult> AddBasketItem(string courseId)
        {
            var course = await _catalogService.GetCourseById(courseId).ConfigureAwait(false);
            var basketItem = new BasketItemDto()
            {
                CourseId = course.Id,
                CourseName = course.Name,
                Price = course.Price
            };
            await _basketService.AddBasketItem(basketItem).ConfigureAwait(false);
            return RedirectToAction(nameof(BasketController.Index),"Basket");
        }

        public async Task<IActionResult> DeleteBasketItem(string courseId)
        {
            await _basketService.RemoveBasketItem(courseId).ConfigureAwait(false);
            return RedirectToAction(nameof(BasketController.Index), "Basket");
        }

        public async Task<IActionResult> CancelDiscount()
        {
            await _basketService.CancelDiscount().ConfigureAwait(false);
            return RedirectToAction(nameof(BasketController.Index), controllerName: "Basket");
        }

        public async Task<IActionResult> ApplyDiscount(DiscountApplyInput data)
        {
            if(!ModelState.IsValid)
            {
                TempData["discountError"] = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).FirstOrDefault();
                return RedirectToAction(nameof(BasketController.Index), controllerName: "Basket");
            }
            var discountResult = await _basketService.ApplyDiscount(data.Code).ConfigureAwait(false);
            TempData["discountResult"] = discountResult;
            return RedirectToAction(nameof(BasketController.Index), controllerName: "Basket");
        }
    }
}
