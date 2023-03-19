using FreeCourse.Web.DTOs.PhotoStocks;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FreeCourse.Web.Abstractions
{
    public interface IPhotoStockService
    {
        Task<PhotoDto> UploadPhoto(IFormFile photo);
        Task<bool> DeletePhoto(string url);
    }
}
