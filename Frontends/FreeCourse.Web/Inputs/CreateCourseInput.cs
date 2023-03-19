using FreeCourse.Web.DTOs.Catalogs;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Inputs
{
    public class CreateCourseInput
    {
        [Display(Name = "Kurs adı")]
        public string Name { get; set; }
        [Display(Name = "Kurs açıklaması")]
        public string Description { get; set; }
        [Display(Name = "Kurs fiyatı")]
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string Picture { get; set; }
        public FeatureDto Feature { get; set; }
        [Display(Name = "Kurs Kategorisi")]
        public string CategoryId { get; set; }
        [Display(Name = "Kurs Resmi")]
        public IFormFile Photo { get; set; }
    }
}
