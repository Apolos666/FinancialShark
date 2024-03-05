using api.DTOs.CommentDTO;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICommentRepository _commentRepository;
    private readonly IStockRepository _stockRepository;

    public CommentController(IMapper mapper, ICommentRepository commentRepository, IStockRepository stockRepository)
    {
        _mapper = mapper;
        _commentRepository = commentRepository;
        _stockRepository = stockRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var comments = await _commentRepository.GetAllAsync();

        var commentsDTO = comments.Select(c => _mapper.Map<CommentDTO>(c));
        
        return Ok(commentsDTO);
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);

        if (comment is null)
            return NotFound();

        return Ok(_mapper.Map<CommentDTO>(comment));
    }
    
    [HttpPost]
    [Route("{stockId:int}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDTO createCommentDTO)
    {
        if (!await _stockRepository.StockExists(stockId))
            return BadRequest("Stock does not exist");
        
        var commentModel = _mapper.Map<Comment>(createCommentDTO);
        
        commentModel.StockId = stockId;
        
        await _commentRepository.CreateAsync(commentModel);
        
        return CreatedAtAction(
            nameof(GetById), 
            new { id = commentModel.Id },
            _mapper.Map<CommentDTO>(commentModel));
    }
    
    [HttpPut]
    [Route("{commentId:int}")]
    public async Task<IActionResult> Update([FromRoute] int commentId, [FromBody] UpdateCommentDTO updateCommentDTO)
    {
        var comment = await _commentRepository.UpdateAsync(commentId, updateCommentDTO);
        
        if (comment is null)
            return NotFound();
        
        return Ok(_mapper.Map<CommentDTO>(comment));
    }
    
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var commentModel = await _commentRepository.DeleteAsync(id);
        
        if (commentModel is null)
            return NotFound($"Comment with id: {id} does not exist");
    
        return NoContent();
    }
}
