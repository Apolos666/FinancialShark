using api.Data;
using api.DTOs.StockDTO;
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
}