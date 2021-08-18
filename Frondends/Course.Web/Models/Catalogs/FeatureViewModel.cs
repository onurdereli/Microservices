using System.ComponentModel.DataAnnotations;

namespace Course.Web.Models.Catalogs
{
    public class FeatureViewModel
    {
        [Display(Name = "Kurs süresi")]
        [Required]
        public int Duration { get; set; }
    }
}
