using FreeCourse.Web.DTOs.Baskets;
using System.Threading.Tasks;

namespace FreeCourse.Web.Abstractions
{
    public interface IBasketService
    {
        Task<bool> SaveOrUpdateBasketAsync(BasketDto data);
        Task<BasketDto> GetBasketAsync();
        Task<bool> DeleteBasketAsync();
        Task AddBasketItem(BasketItemDto data);
        Task<bool> RemoveBasketItem(string courseId);
        Task<bool> ApplyDiscount(string discountCode);
        Task<bool> CancelDiscount();
    }
}
