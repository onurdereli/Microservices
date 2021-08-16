using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Course.Shared.Dtos;
using Course.Web.Models.PhotoStocks;
using Course.Web.Services.Abstract;
using Microsoft.AspNetCore.Http;

namespace Course.Web.Services.Concrete
{
    public class PhotoStockService : IPhotoStockService
    {
        private readonly HttpClient _httpClient;

        public PhotoStockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PhotoViewModel> UploadPhoto(IFormFile photo)
        {
            if (photo is not { Length: > 0 })
            {
                return null;
            }

            var randomFilename = $"{Guid.NewGuid().ToString()}{Path.GetExtension(photo.FileName)}";

            await using MemoryStream ms = new();
            await photo.CopyToAsync(ms);

            MultipartFormDataContent multipartFormDataContent = new();

            multipartFormDataContent.Add(new ByteArrayContent(ms.ToArray()), "photoFile", randomFilename);

            var response = await _httpClient.PostAsync("photos", multipartFormDataContent);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<PhotoViewModel>>();
            return responseSuccess?.Data;
        }

        public async Task<bool> DeletePhoto(string pictureUrl)
        {
            var response = await _httpClient.DeleteAsync($"photos?photoUrl={pictureUrl}");

            return response.IsSuccessStatusCode;
        }
    }
}
