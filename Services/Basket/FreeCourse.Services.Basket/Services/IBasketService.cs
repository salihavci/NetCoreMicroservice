using FreeCourse.Services.Basket.DTOs;
using FreeCourse.Shared.Dtos;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Services
{
    public interface IBasketService
    {
        Task<Response<BasketVM>> GetBasket(string userId);
        Task<Response<bool>> SaveOrUpdate(BasketVM data);
        Task<Response<bool>> Delete(string userId);
    }
}
