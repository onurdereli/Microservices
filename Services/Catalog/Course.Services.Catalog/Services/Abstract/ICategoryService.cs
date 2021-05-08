using System.Collections.Generic;
using System.Threading.Tasks;
using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Model;
using Course.Shared.Dtos;

namespace Course.Services.Catalog.Services.Abstract
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDto>>> GetAllAsync();
        Task<Response<CategoryDto>> CreateAsync(CategoryDto category);
        Task<Response<CategoryDto>> GetByIdAsync(string id);
    }
}
