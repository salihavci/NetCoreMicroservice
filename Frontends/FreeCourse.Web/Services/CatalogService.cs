using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Abstractions;
using FreeCourse.Web.DTOs.Catalogs;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Inputs;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;
        private readonly IPhotoStockService _photostockService;
        private readonly PhotoHelper _photoHelper;

        public CatalogService(HttpClient client, IPhotoStockService photostockService, PhotoHelper photoHelper)
        {
            _client = client;
            _photostockService = photostockService;
            _photoHelper = photoHelper;
        }

        public async Task<bool> CreateCourseAsync(CreateCourseInput data)
        {
            var photoSaveResult = await _photostockService.UploadPhoto(data.Photo).ConfigureAwait(false);
            if(photoSaveResult != null)
            {
                data.Picture = photoSaveResult.Url;
            }
            var response = await _client.PostAsJsonAsync("courses", data).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourseAsync(string courseId)
        {
            var response = await _client.DeleteAsync($"courses/{courseId}").ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var response = await _client.GetAsync("categories").ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var result = await response.Content.ReadFromJsonAsync<Response<List<CategoryDto>>>().ConfigureAwait(false);
            return result.Data;
        }

        public async Task<List<CourseDto>> GetAllCourseAsync()
        {
            var response = await _client.GetAsync("courses").ConfigureAwait(false);
            if(!response.IsSuccessStatusCode)
            {
                return null;
            }
            var result = await response.Content.ReadFromJsonAsync<Response<List<CourseDto>>>().ConfigureAwait(false);
            result.Data.ForEach(x =>
            {
                x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
            });
            return result.Data;
        }

        public async Task<List<CourseDto>> GetAllCourseByUserIdAsync(string userId)
        {
            var response = await _client.GetAsync($"courses/GetAllByUserIdAsync/{userId}").ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var result = await response.Content.ReadFromJsonAsync<Response<List<CourseDto>>>().ConfigureAwait(false);
            result.Data.ForEach(x =>
            {
                x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
            });
            return result.Data;
        }

        public async Task<CourseDto> GetCourseById(string courseId)
        {
            var response = await _client.GetAsync($"courses/{courseId}").ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var result = await response.Content.ReadFromJsonAsync<Response<CourseDto>>().ConfigureAwait(false);
            result.Data.StockPictureUrl = _photoHelper.GetPhotoStockUrl(result.Data.Picture);
            return result.Data;
        }

        public async Task<bool> UpdateCourseAsync(UpdateCourseInput data)
        {
            var photoSaveResult = await _photostockService.UploadPhoto(data.Photo).ConfigureAwait(false);
            if (photoSaveResult != null)
            {
                await _photostockService.DeletePhoto(data.Picture).ConfigureAwait(false);
                data.Picture = photoSaveResult.Url;
            }
            var response = await _client.PutAsJsonAsync("courses", data).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }
    }
}
