using FreeCourse.Services.Photostock.DTOs;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Services.Photostock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
        {
            if(photo != null && photo.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await photo.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);
                }
                var fileName = photo.FileName;
                var photoDto = new PhotoDto
                {
                    Url = fileName
                };
                return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto,200));
            }

            return CreateActionResultInstance(Response<PhotoDto>.Fail("Photo is empty.",400));
        }

        [HttpDelete]
        public IActionResult PhotoDelete([FromQuery] string photoUrl, CancellationToken cancellationToken)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);
            if(!System.IO.File.Exists(path))
            {
                return CreateActionResultInstance(Response<NoContent>.Fail("Photo not found.", NotFound().StatusCode));
            }
            System.IO.File.Delete(path);
            return CreateActionResultInstance(Response<NoContent>.Success(NoContent().StatusCode));
        }
    }
}
