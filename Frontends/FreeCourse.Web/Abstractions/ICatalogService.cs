using FreeCourse.Web.DTOs.Catalogs;
using FreeCourse.Web.Inputs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Web.Abstractions
{
    public interface ICatalogService
    {
        Task<List<CourseDto>> GetAllCourseAsync();
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<List<CourseDto>> GetAllCourseByUserIdAsync(string userId);
        Task<CourseDto> GetCourseById(string courseId);
        Task<bool> CreateCourseAsync(CreateCourseInput data);
        Task<bool> UpdateCourseAsync(UpdateCourseInput data);
        Task<bool> DeleteCourseAsync(string courseId);
        
        
    }
}
