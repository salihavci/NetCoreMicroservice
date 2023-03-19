using FreeCourse.Web.Abstractions;
using FreeCourse.Web.DTOs.PhotoStocks;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Json;
using FreeCourse.Shared.Dtos;

namespace FreeCourse.Web.Services
{
    public class PhotoStockService : IPhotoStockService
    {
        private readonly HttpClient _client;

        public PhotoStockService(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> DeletePhoto(string url)
        {
            var response = await _client.DeleteAsync($"photos?photoUrl={url}").ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoDto> UploadPhoto(IFormFile photo)
        {
            if(photo == null || photo.Length <= 0)
            {
                return null;
            }
            var randomFileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
            using (var ms = new MemoryStream())
            {
                await photo.CopyToAsync(ms).ConfigureAwait(false);

                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(new ByteArrayContent(ms.ToArray()), "photo", randomFileName);
                var response = await _client.PostAsync("photos", multipartContent).ConfigureAwait(false);
                if(!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var result = await response.Content.ReadFromJsonAsync<Response<PhotoDto>>().ConfigureAwait(false);
                return result.Data;
            }
        }
    }
}
