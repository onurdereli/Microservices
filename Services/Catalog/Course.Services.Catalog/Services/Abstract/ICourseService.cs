using System.Collections.Generic;
using System.Threading.Tasks;
using Course.Services.Catalog.Dtos;
using Course.Shared.Dtos;

namespace Course.Services.Catalog.Services.Abstract
{
    public interface ICourseService
    {
        Task<Response<List<CourseDto>>> GetAllAsync();
        Task<Response<CourseDto>> GetByIdAsync(string id);
        Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId);
        Task<Response<CourseDto>> CreateAsync(CourseCreateDto couseCreateDto);
        Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto);
        Task<Response<NoContent>> DeleteAysnc(string id);
    }
}
