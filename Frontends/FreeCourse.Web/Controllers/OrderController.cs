using FreeCourse.Web.Abstractions;
using FreeCourse.Web.Inputs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;

        public OrderController(IBasketService basketService, IOrderService orderService, IPaymentService paymentService)
        {
            _basketService = basketService;
            _orderService = orderService;
            _paymentService = paymentService;
        }

        public async Task<IActionResult> Checkout()
        {
            var basket = await _basketService.GetBasketAsync().ConfigureAwait(false);
            ViewBag.basket = basket;
            return View(new CheckoutInfoInput());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutInfoInput data)
        {
            if (!ModelState.IsValid)
            {
                var basket = await _basketService.GetBasketAsync().ConfigureAwait(false);
                ViewBag.basket = basket;
                return View(data);
            }
            var orderStatus = await _orderService.CreateOrder(data).ConfigureAwait(false);
            if (!orderStatus.IsSuccessful)
            {
                var basket = await _basketService.GetBasketAsync().ConfigureAwait(false);
                ViewBag.basket = basket;
                ViewBag.error = orderStatus.Error;
                return View();
            }

            return RedirectToAction(nameof(OrderController.SuccessfulCheckout),"Order",new { orderId = orderStatus.OrderId });
        }

        public IActionResult SuccessfulCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }


    }
}
