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
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<UserToCreateDto, User>();
            CreateMap<User, UserToCreateDto> ();
        }
    }
}
