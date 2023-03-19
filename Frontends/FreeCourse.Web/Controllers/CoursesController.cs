using FreeCourse.Shared.Services;
using FreeCourse.Web.Abstractions;
using FreeCourse.Web.Inputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace FreeCourse.Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public CoursesController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await _catalogService.GetAllCourseByUserIdAsync(_sharedIdentityService.GetUserId).ConfigureAwait(false);
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await GetCategories().ConfigureAwait(false);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseInput data)
        {
            await GetCategories().ConfigureAwait(false);

            if (!ModelState.IsValid)
            {
                return View(data);
            }
            data.UserId = _sharedIdentityService.GetUserId;
            var response = await _catalogService.CreateCourseAsync(data).ConfigureAwait(false);
            return RedirectToAction(nameof(Index), "Courses");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var course = await _catalogService.GetCourseById(id).ConfigureAwait(false);
            if (course == null)
            {
                return RedirectToAction(nameof(Index), "Courses");
            }
            await GetCategories(course.Id).ConfigureAwait(false);

            var result = new UpdateCourseInput()
            {
                Id = course.Id,
                CategoryId = course.CategoryId,
                Description = course.Description,
                Name = course.Name,
                Feature = course.Feature,
                Picture = course.Picture,
                Price = course.Price,
                UserId = course.UserId
            };

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCourseInput data)
        {
            await GetCategories(data.Id).ConfigureAwait(false);
            if (!ModelState.IsValid) 
            {
                return View(data); 
            }
            var response = await _catalogService.UpdateCourseAsync(data).ConfigureAwait(false);
            return RedirectToAction(nameof(Index), "Courses");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _catalogService.DeleteCourseAsync(id).ConfigureAwait(false);
            return RedirectToAction(nameof(Index), "Courses");
        }

        private async Task GetCategories(object selectedValue = null)
        {
            var categories = await _catalogService.GetAllCategoriesAsync().ConfigureAwait(false);
            if(selectedValue == null)
            {
                ViewBag.CategoryList = new SelectList(categories, "Id", "Name");
            }
            else
            {
                ViewBag.CategoryList = new SelectList(categories, "Id", "Name", selectedValue);
            }
        }

    }
}
