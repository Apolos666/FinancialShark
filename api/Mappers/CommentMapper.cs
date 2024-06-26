﻿using api.DTOs.CommentDTO;
using api.Models;

namespace api.Mappers;

public static class CommentMapper
{
    public static CommentDTO toCommentDTO(this Comment commentModel)
    {
        return new CommentDTO
        {
            Id = commentModel.Id,
            Title = commentModel.Title,
            Content = commentModel.Content,
            CreatedOn = commentModel.CreatedOn,
            StockId = commentModel.StockId
        };
    }
    
    public static Comment toCommentFromCreate(this CreateCommentDTO createCommentDTO, int stockId)
    {
        return new Comment
        {
            Title = createCommentDTO.Title,
            Content = createCommentDTO.Content,
            StockId = stockId
        };
    }
}