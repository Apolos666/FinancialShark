using api.DTOs.CommentDTO;
using api.Models;
using AutoMapper;

namespace api.Mappers;

public class CommentAutoMappers : Profile
{
    public CommentAutoMappers()
    {
        CreateMap<Comment, CommentDTO>()
            .ForMember(dest => dest.UserName, 
                opt => opt.MapFrom(src => src.AppUser.UserName));
        CreateMap<CreateCommentDTO, Comment>();
        CreateMap<UpdateCommentDTO, Comment>();
    }
} 