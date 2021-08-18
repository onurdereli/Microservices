using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.Web.Models.Catalogs;

namespace Course.Web.Services.Abstract
{
    public interface ICatalogService
    {
        Task<List<CourseViewModel>> GetAllCourse();

        Task<List<CategoryViewModel>> GetAllGategory();

        Task<List<CourseViewModel>> GetAllCourseByUserId(string userId);

        Task<CourseViewModel> GetByCourseId(string courseId);

        Task<bool> CreateCourse(CourseCreateInput courseCreateInput);

        Task<bool> UpdateCourse(CourseUpdateInput courseUpdateInput);

        Task<bool> DeleteCourse(string courseId);
    }
}
