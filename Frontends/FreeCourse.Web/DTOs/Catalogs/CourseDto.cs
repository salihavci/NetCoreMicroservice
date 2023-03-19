using System;

namespace FreeCourse.Web.DTOs.Catalogs
{
    public class CourseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string Picture { get; set; }
        public DateTime CreatedTime { get; set; }
        public FeatureDto Feature { get; set; }
        public string CategoryId { get; set; }
        public CategoryDto Category { get; set; }
        public string ShortDescription { get => Description.Length > 100 ? Description.Substring(0, 100) + "..." : Description; }
        public string StockPictureUrl { get; set; }
    }
}
