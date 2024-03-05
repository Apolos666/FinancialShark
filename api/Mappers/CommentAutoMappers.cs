using api.DTOs.CommentDTO;
using api.Models;
using AutoMapper;

namespace api.Mappers;

public class CommentAutoMappers : Profile
{
    public CommentAutoMappers()
    {
        CreateMap<Comment, CommentDTO>();
        CreateMap<CreateCommentDTO, Comment>();
        CreateMap<UpdateCommentDTO, Comment>();
    }
}