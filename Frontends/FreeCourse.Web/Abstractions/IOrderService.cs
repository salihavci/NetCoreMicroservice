using FreeCourse.Web.DTOs.Orders;
using FreeCourse.Web.Inputs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Web.Abstractions
{
    public interface IOrderService
    {
        /// <summary>
        /// Senkron iletişim için. Sipariş bilgileri sadece Order MicroserviceAPI'sine istek atacak.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<OrderCreatedDto> CreateOrder(CheckoutInfoInput data);

        /// <summary>
        /// Asenkron iletişim için. Sipariş bilgileri RabbitMQ ile işlenecek
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<OrderSuspendedDto> SuspendOrder(CheckoutInfoInput data);
        Task<List<OrderDto>> GetOrder();

    }
}
