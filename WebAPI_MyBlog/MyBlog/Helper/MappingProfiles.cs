using AutoMapper;
using MyBlog.Dto;
using MyBlog.Models;
using MyBlog_API.Dto;

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
            CreateMap<Post, PostDto>();
            CreateMap<PostDto, Post>();
            CreateMap<PostToAddDto, Post>();
            CreateMap<CommentDto, Comment>();
            CreateMap<Comment, CommentDto>();
        }
    }
}
