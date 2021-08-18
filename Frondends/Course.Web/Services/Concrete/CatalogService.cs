using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Course.Shared.Dtos;
using Course.Web.Helpers;
using Course.Web.Models.Catalogs;
using Course.Web.Services.Abstract;

namespace Course.Web.Services.Concrete
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;

        private readonly IPhotoStockService _photoStockService;

        private readonly PhotoHelper _photoHelper;

        public CatalogService(HttpClient httpClient, IPhotoStockService photoStockService, PhotoHelper photoHelper)
        {
            _httpClient = httpClient;
            _photoStockService = photoStockService;
            _photoHelper = photoHelper;
        }
        
        public async Task<List<CourseViewModel>> GetAllCourse()
        {
            var response = await _httpClient.GetAsync("course");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            responseSuccess?.Data.ForEach(course=>
            {
                course.StockPictureUrl = _photoHelper.GetPhotoStockUrl(course.Picture);
            });

            return responseSuccess?.Data;
        }

        public async Task<List<CategoryViewModel>> GetAllGategory()
        {
            var response = await _httpClient.GetAsync("category");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();

            return responseSuccess?.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseByUserId(string userId)
        {
            //[controller]/GetAllByUserId/{userId}
            var response = await _httpClient.GetAsync($"course/GetAllByUserId/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            responseSuccess?.Data.ForEach(course =>
            {
                course.StockPictureUrl = _photoHelper.GetPhotoStockUrl(course.Picture);
            });

            return responseSuccess?.Data;
        }

        public async Task<CourseViewModel> GetByCourseId(string courseId)
        {
            //{id}
            var response = await _httpClient.GetAsync($"course/{courseId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();
            responseSuccess.Data.StockPictureUrl = _photoHelper.GetPhotoStockUrl(responseSuccess.Data.Picture);
            return responseSuccess.Data;
        }

        public async Task<bool> CreateCourse(CourseCreateInput courseCreateInput)
        {
            var resultPhoto = await _photoStockService.UploadPhoto(courseCreateInput.PhotoFormFile);

            if(resultPhoto != null)
            {
                courseCreateInput.Picture = resultPhoto.Url;
            }

            var response = await _httpClient.PostAsJsonAsync<CourseCreateInput>("course", courseCreateInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCourse(CourseUpdateInput courseUpdateInput)
        {
            var resultPhoto = await _photoStockService.UploadPhoto(courseUpdateInput.PhotoFormFile);

            if (resultPhoto != null)
            {
                await _photoStockService.DeletePhoto(courseUpdateInput.Picture);

                courseUpdateInput.Picture = resultPhoto.Url;
            }

            var response = await _httpClient.PutAsJsonAsync<CourseUpdateInput>("course", courseUpdateInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourse(string courseId)
        {
            var response = await _httpClient.DeleteAsync($"course/{courseId}");

            return response.IsSuccessStatusCode;
        }
    }
}
