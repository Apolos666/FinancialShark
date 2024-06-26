﻿using api.DTOs.CommentDTO;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    private readonly IFMPService _fmpService;

    public CommentController(UserManager<AppUser> userManager,IMapper mapper, ICommentRepository commentRepository, IStockRepository stockRepository, IFMPService fmpService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _commentRepository = commentRepository;
        _stockRepository = stockRepository;
        _fmpService = fmpService;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] CommentQueryObject queryObject)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var comments = await _commentRepository.GetAllAsync(queryObject);

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
    [Route("{symbol:alpha}")]
    public async Task<IActionResult> Create([FromRoute] string symbol, [FromBody] CreateCommentDTO createCommentDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var stock = await _stockRepository.GetBySymbolAsync(symbol);

        if (stock is null)
        {
            stock = await _fmpService.FindStockBySymbolAsync(symbol);
            if (stock is null)
                return BadRequest("Stock does not exist in the database or FMP API.");
            else
                await _stockRepository.CreateAsync(stock);
        }
        
        var userName = User.GetUserName();
        var appUser = await _userManager.FindByNameAsync(userName);
        
        var commentModel = _mapper.Map<Comment>(createCommentDTO);
        
        commentModel.StockId = stock.Id;
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
