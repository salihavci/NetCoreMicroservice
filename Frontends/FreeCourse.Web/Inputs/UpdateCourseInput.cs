using FreeCourse.Web.DTOs.Catalogs;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Inputs
{
    public class UpdateCourseInput
    {
        public string Id { get; set; }
        [Display(Name = "Kurs adı")]
        public string Name { get; set; }
        [Display(Name = "Kurs açıklaması")]
        public string Description { get; set; }
        [Display(Name = "Kurs Fiyatı")]
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string Picture { get; set; }
        public FeatureDto Feature { get; set; }
        [Display(Name = "Kurs kategorisi")]
        public string CategoryId { get; set; }

        [Display(Name = "Kurs resmi")]
        public IFormFile Photo { get; set; }
    }
}
