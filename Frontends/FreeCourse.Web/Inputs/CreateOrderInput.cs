using FreeCourse.Web.DTOs.Orders;
using System.Collections.Generic;

namespace FreeCourse.Web.Inputs
{
    public class CreateOrderInput
    {
        public CreateOrderInput()
        {
            OrderItems = new List<OrderItemDto>();
        }
        public string BuyerId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public AddressDto Address { get; set; }
    }
}
