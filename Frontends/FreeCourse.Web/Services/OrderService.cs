using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Abstractions;
using FreeCourse.Web.DTOs.Orders;
using FreeCourse.Web.Inputs;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _client;
        private readonly IBasketService _basketService;
        private readonly IPaymentService _paymentService;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly ICatalogService _catalogService;

        public OrderService(HttpClient client, IBasketService basketService, IPaymentService paymentService, ISharedIdentityService sharedIdentityService, ICatalogService catalogService)
        {
            _client = client;
            _basketService = basketService;
            _paymentService = paymentService;
            _sharedIdentityService = sharedIdentityService;
            _catalogService = catalogService;
        }

        public async Task<OrderCreatedDto> CreateOrder(CheckoutInfoInput data)
        {
            var basket = await _basketService.GetBasketAsync().ConfigureAwait(false);
            var payment = new PaymentInfoInput()
            {
                CardName = data.CardName,
                CardNumber = data.CardNumber,
                CVV = data.CVV,
                Expiration = data.Expiration,
                TotalPrice = basket.TotalPrice
            };

            var responsePayment = await _paymentService.ReceivePayment(payment).ConfigureAwait(false);
            if(!responsePayment)
            {
                return new OrderCreatedDto()
                {
                    Error = "Ödeme alınamadı.",
                    IsSuccessful = false
                };
            }

            var orderRequest = new CreateOrderInput()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new AddressDto()
                {
                    Province = data.Province,
                    District = data.District,
                    Line = data.Line,
                    Street = data.Street,
                    ZipCode = data.ZipCode
                }
            };
            basket.BasketItems.ForEach(b =>
            {
                var coursePicture = _catalogService.GetCourseById(b.CourseId).Result;
                var item = new OrderItemDto()
                {
                    ProductId = b.CourseId,
                    Price = b.GetCurrentPrice,
                    PictureUrl = coursePicture.StockPictureUrl,
                    ProductName = b.CourseName
                };

                orderRequest.OrderItems.Add(item); 
            });

            var response = await _client.PostAsJsonAsync("orders",orderRequest).ConfigureAwait(false);
            if(!response.IsSuccessStatusCode)
            {
                return new OrderCreatedDto()
                {
                    Error = "Sipariş oluşturulamadı.",
                    IsSuccessful = false
                };
            }

            var orderResult = await response.Content.ReadFromJsonAsync<Response<OrderCreatedDto>>().ConfigureAwait(false);
            orderResult.Data.IsSuccessful = true;
            var deleteBasketResponse = await _basketService.DeleteBasketAsync().ConfigureAwait(false);
            return orderResult.Data;
        }

        public async Task<List<OrderDto>> GetOrder()
        {
            var response = await _client.GetFromJsonAsync<Response<List<OrderDto>>>("orders").ConfigureAwait(false);
            return response.Data;
        }

        public async Task<OrderSuspendedDto> SuspendOrder(CheckoutInfoInput data)
        {
            var basket = await _basketService.GetBasketAsync().ConfigureAwait(false);
            
            var orderRequest = new CreateOrderInput()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new AddressDto()
                {
                    Province = data.Province,
                    District = data.District,
                    Line = data.Line,
                    Street = data.Street,
                    ZipCode = data.ZipCode
                }
            };
            basket.BasketItems.ForEach(b =>
            {
                var coursePicture = _catalogService.GetCourseById(b.CourseId).Result;
                var item = new OrderItemDto()
                {
                    ProductId = b.CourseId,
                    Price = b.GetCurrentPrice,
                    PictureUrl = coursePicture.StockPictureUrl,
                    ProductName = b.CourseName
                };

                orderRequest.OrderItems.Add(item);
            });

            var payment = new PaymentInfoInput()
            {
                CardName = data.CardName,
                CardNumber = data.CardNumber,
                Expiration = data.Expiration,
                CVV = data.CVV,
                TotalPrice = basket.TotalPrice,
                Order = orderRequest
            };

            var responsePayment = await _paymentService.ReceivePayment(payment).ConfigureAwait(false);
            if(!responsePayment)
            {
                return new OrderSuspendedDto()
                {
                    Error = "Ödeme alınamadı.",
                    IsSuccessful = false
                };
            }

            var deleteBasketResponse = await _basketService.DeleteBasketAsync().ConfigureAwait(false);
            return new OrderSuspendedDto()
            {
                IsSuccessful = true
            };
        }
    }
}
