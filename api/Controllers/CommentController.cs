using api.DTOs.CommentDTO;
using api.Extensions;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ICommentRepository _commentRepository;
    private readonly IStockRepository _stockRepository;

    public CommentController(UserManager<AppUser> userManager,IMapper mapper, ICommentRepository commentRepository, IStockRepository stockRepository)
    {
        _userManager = userManager;
        _mapper = mapper;
        _commentRepository = commentRepository;
        _stockRepository = stockRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var comments = await _commentRepository.GetAllAsync();

        var commentsDTO = comments.Select(c => _mapper.Map<CommentDTO>(c));
        
        return Ok(commentsDTO);
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var comment = await _commentRepository.GetByIdAsync(id);

        if (comment is null)
            return NotFound();

        return Ok(_mapper.Map<CommentDTO>(comment));
    }
    
    [HttpPost]
    [Route("{stockId:int}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDTO createCommentDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if (!await _stockRepository.StockExists(stockId))
            return BadRequest("Stock does not exist");  
        
        var userName = User.GetUserName();
        var appUser = await _userManager.FindByNameAsync(userName);
        
        var commentModel = _mapper.Map<Comment>(createCommentDTO);
        
        commentModel.StockId = stockId;
        commentModel.AppUserId = appUser.Id;
        
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
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var comment = await _commentRepository.UpdateAsync(commentId, updateCommentDTO);
        
        if (comment is null)
            return NotFound();
        
        return Ok(_mapper.Map<CommentDTO>(comment));
    }
    
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var commentModel = await _commentRepository.DeleteAsync(id);
        
        if (commentModel is null)
            return NotFound($"Comment with id: {id} does not exist");
    
        return NoContent();
    }
}
