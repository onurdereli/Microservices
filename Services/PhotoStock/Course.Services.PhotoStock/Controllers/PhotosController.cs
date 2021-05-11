using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Course.Services.PhotoStock.Dtos;
using Course.Shared.ControllerBase;
using Course.Shared.Dtos;

namespace Course.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {
        // CancellationToken; Dosya kaydetme işlemi yaparken endpoint işlemi bitince bitmesini sağlar
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photoFile, CancellationToken cancellationToken)
        {
            if (photoFile == null || photoFile.Length <= 0)
            {
                return CreateActionResultInstance(Response<PhotoDto>.Fail("Photo is empty", 400));
            }   

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoFile.FileName);

            await using var steam = new FileStream(path, FileMode.Create);
            await photoFile.CopyToAsync(steam, cancellationToken);

            var returnPath = "photos/" + photoFile.FileName;

            PhotoDto photoDto = new() { Url = returnPath};

            return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto, 200));
        }

        [HttpDelete]
        public IActionResult PhotoDelete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos", photoUrl);

            if (!System.IO.File.Exists(path))
            {
                return CreateActionResultInstance(Response<NoContent>.Fail("Photo not found", 404));
            }

            System.IO.File.Delete(path);

            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }
    }
}
