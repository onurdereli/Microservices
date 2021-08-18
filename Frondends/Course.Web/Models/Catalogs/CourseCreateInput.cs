using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Course.Web.Models.Catalogs
{
    public class CourseCreateInput
    {
        [Display(Name = "Kurs ismi")]
        public string Name { get; set; }

        [Display(Name = "Kurs açıklaması")]
        public string Description { get; set; }

        [Display(Name = "Kurs fiyatı")]
        public decimal Price { get; set; }

        public string UserId { get; set; }

        public string Picture { get; set; }

        public FeatureViewModel Feature { get; set; }

        [Display(Name = "Kurs kategorisi")]
        public string CategoryId { get; set; }

        [Display(Name = "Kurs Resmi")]
        public IFormFile PhotoFormFile { get; set; }
    }
}
