﻿using FreeCourse.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discount.Services
{
    public interface IDiscountService
    {
        Task<Response<List<Models.Discount>>> GetListAsync();
        Task<Response<Models.Discount>> GetByIdAsync(int id);
        Task<Response<NoContent>> SaveAsync(Models.Discount data);
        Task<Response<NoContent>> UpdateAsync(Models.Discount data);
        Task<Response<NoContent>> DeleteAsync(int id);
        Task<Response<Models.Discount>> GetByCodeAndUserIdAsync(string code, string userId);
    }
}
