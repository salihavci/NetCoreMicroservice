using FreeCourse.Web.DTOs.Discounts;
using System.Threading.Tasks;

namespace FreeCourse.Web.Abstractions
{
    public interface IDiscountService
    {
        Task<DiscountDto> GetDiscount(string discountCode);
    }
}
