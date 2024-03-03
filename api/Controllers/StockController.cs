using api.Data;
using api.DTOs.StockDTO;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ApplicationDBContext _context;

    public StockController(IMapper mapper ,ApplicationDBContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var stocks = _context.Stocks.ToList().Select(s => _mapper.Map<StockDTO>(s));

        return Ok(stocks);
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var stock = _context.Stocks.Find(id);

        if (stock is null)
            return NotFound();

        return Ok(_mapper.Map<StockDTO>(stock));
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateStockRequestDTO createStockDTO)
    {
        var stockModel = _mapper.Map<Stock>(createStockDTO);
        _context.Stocks.Add(stockModel);
        _context.SaveChanges();
        return CreatedAtAction(
            nameof(GetById), 
            new { id = stockModel.Id },
            _mapper.Map<StockDTO>(stockModel));
    }
    
    [HttpPut]
    [Route("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateStockDTO)
    {
        var stock = _context.Stocks.FirstOrDefault(s => s.Id == id);

        if (stock is null)
            return NotFound();
        
        _mapper.Map(updateStockDTO, stock);
        _context.SaveChanges();
        return Ok(_mapper.Map<StockDTO>(stock));
    }
    
    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var stockModel = _context.Stocks.FirstOrDefault(s => s.Id == id);

        if (stockModel is null)
            return NotFound();
        
        _context.Stocks.Remove(stockModel);
        _context.SaveChanges();
        return NoContent();
    }
}