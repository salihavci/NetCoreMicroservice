using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.DTOs.Catalogs
{
    public class FeatureDto
    {
        [Display(Name = "Kursun Süresi")]
        [Required]
        public int Duration { get; set; }

    }
}
