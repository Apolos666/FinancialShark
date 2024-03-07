using api.Data;
using api.DTOs.CommentDTO;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly IMapper _mapper;
    private readonly ApplicationDBContext _context;

    public CommentRepository(IMapper mapper, ApplicationDBContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    
    public async Task<List<Comment>> GetAllAsync()
    {
        return await _context.Comments
            .Include(c => c.AppUser)
            .ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _context.Comments
            .Include(c => c.AppUser)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Comment> CreateAsync(Comment createComment)
    {
        await _context.Comments.AddAsync(createComment);
        await _context.SaveChangesAsync();
        return createComment;
    }

    public async Task<Comment?> UpdateAsync(int id, UpdateCommentDTO updateComment)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        
        if (comment is null)
            return null;
        
        _mapper.Map(updateComment, comment);
        
        comment.CreatedOn = DateTime.Now;

        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<Comment?> DeleteAsync(int id)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        
        if (comment is null)
            return null;
        
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<bool> CommentExists(int id)
    {
        return await _context.Comments.AnyAsync(c => c.Id == id);
    }
}