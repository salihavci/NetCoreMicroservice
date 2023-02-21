using AutoMapper;
using FreeCourse.Services.Order.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Mappings
{
    public class CustomMapping: Profile
    {
        public CustomMapping()
        {
            this.CreateMap<Domain.OrderAggregate.Order, OrderDto>().ReverseMap();
            this.CreateMap<Domain.OrderAggregate.OrderItem, OrderItemDto>().ReverseMap();
            this.CreateMap<Domain.OrderAggregate.Address, AddressDto>().ReverseMap();
        }
    }
}
