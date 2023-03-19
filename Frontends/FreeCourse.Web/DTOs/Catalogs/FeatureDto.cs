using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.DTOs.Catalogs
{
    public class FeatureDto
    {
        [Display(Name = "Kursun Süresi")]
        public int Duration { get; set; }

    }
}
