using AutoMapper;
using MyBlog.Dto;
using MyBlog.Models;

namespace MyBlog.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
        }
    }
}
