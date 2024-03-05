﻿using api.Data;
using api.DTOs.StockDTO;
using api.Interfaces;
using api.Mappers;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IStockRepository _stockRepository;

    public StockController(IMapper mapper ,IStockRepository stockRepository)
    {
        _mapper = mapper;
        _stockRepository = stockRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stocks = await _stockRepository.GetAllAsync();
            
        var stocksDTO = stocks.Select(s => _mapper.Map<StockDTO>(s));

        return Ok(stocksDTO);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var stock = await _stockRepository.GetByIdAsync(id);

        if (stock is null)
            return NotFound();

        return Ok(_mapper.Map<StockDTO>(stock));
    }

    [HttpPost]
    public async Task<CreatedAtActionResult> Create([FromBody] CreateStockRequestDTO createStockDTO)
    {
        var stockModel = _mapper.Map<Stock>(createStockDTO);
        
        await _stockRepository.CreateAsync(stockModel);
        
        return CreatedAtAction(
            nameof(GetById), 
            new { id = stockModel.Id },
            _mapper.Map<StockDTO>(stockModel));
    }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateStockDTO)
    {
        var stock = await _stockRepository.UpdateAsync(id, updateStockDTO);

        if (stock is null)
            return NotFound();
        
        return Ok(_mapper.Map<StockDTO>(stock));
    }
    
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stockModel = await _stockRepository.DeleteAsync(id);
        
        if (stockModel is null)
            return NotFound();
        
        return NoContent();
    }
}