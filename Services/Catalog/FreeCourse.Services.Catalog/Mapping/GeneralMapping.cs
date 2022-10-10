using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;

namespace FreeCourse.Services.Catalog.Mapping
{
    public class GeneralMapping:Profile
    {
        public GeneralMapping()
        {
            this.CreateMap<Course, CourseDto>().ReverseMap();
            this.CreateMap<Category, CategoryDto>().ReverseMap();
            this.CreateMap<Feature, FeatureDto>().ReverseMap();
            this.CreateMap<Course,CourseCreateDto>().ReverseMap();
            this.CreateMap<Course, CourseUpdateDto>().ReverseMap();
        }
    }
}
