using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Course.Shared.Dtos;
using Course.Web.Models.Catalog;
using Course.Web.Services.Abstract;

namespace Course.Web.Services.Concrete
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<List<CourseViewModel>> GetAllCourse()
        {
            var response = await _httpClient.GetAsync("course");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

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

            return responseSuccess?.Data;
        }

        public async Task<bool> CreateCourse(CourseCreateInput courseCreateInput)
        {
            var response = await _httpClient.PostAsJsonAsync<CourseCreateInput>("course", courseCreateInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCourse(CourseUpdateInput courseUpdateInput)
        {
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
