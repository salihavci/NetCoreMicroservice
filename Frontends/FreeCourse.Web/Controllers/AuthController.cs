using FreeCourse.Web.Abstractions;
using FreeCourse.Web.Inputs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Signin(string ReturnUrl = "/")
        {
            TempData["returnUrl"] = ReturnUrl;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _identityService.RevokeRefreshToken().ConfigureAwait(false);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Signin(SigninInput data)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var response = await _identityService.SignInAsync(data).ConfigureAwait(false);
            if (!response.IsSuccessful)
            {
                response.Errors.ForEach(x =>
                {
                    ModelState.AddModelError(string.Empty, x);
                });
                return View();
            }
            if (!string.IsNullOrWhiteSpace(TempData["returnUrl"].ToString()))
            {
                var returnUrl = TempData["returnUrl"].ToString();
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
