using api.DTOs.CommentDTO;
using api.Models;

namespace api.Interfaces;

public interface ICommentRepository
{
    public Task<List<Comment>> GetAllAsync();
    public Task<Comment?> GetByIdAsync(int id);
    public Task<Comment> CreateAsync(Comment createComment);
    public Task<Comment?> UpdateAsync(int id, UpdateCommentDTO updateComment);
    public Task<Comment?> DeleteAsync(int id);
    public Task<bool> CommentExists(int id);
}