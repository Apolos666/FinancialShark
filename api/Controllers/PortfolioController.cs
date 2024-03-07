using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/portfolio")]
[ApiController]
public class PortfolioController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IStockRepository _stockRepository;
    private readonly IPortfolioRepository _portfolioRepository;
    private readonly IFMPService _fmpService;

    public PortfolioController(
        UserManager<AppUser> userManager, 
        IStockRepository stockRepository,
        IPortfolioRepository portfolioRepository,
        IFMPService fmpService
        )
    {
        _userManager = userManager;
        _stockRepository = stockRepository;
        _portfolioRepository = portfolioRepository;
        _fmpService = fmpService;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserPortfolio()
    {
        var userName = User.GetUserName();
        var appUser = await _userManager.FindByNameAsync(userName);
        var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
        return Ok(userPortfolio);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreatePortfolio(string symbol)
    {
        var userName = User.GetUserName();
        var appUser = await _userManager.FindByNameAsync(userName);
        var stock = await _stockRepository.GetBySymbolAsync(symbol);

        if (stock is null)
        {
            stock = await _fmpService.FindStockBySymbolAsync(symbol);
            if (stock is null)
                return BadRequest("Stock does not exist in the database or FMP API.");
            else
                await _stockRepository.CreateAsync(stock);
        }
        
        var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
        
        if (userPortfolio.Any(s => s.Symbol.ToLower() == symbol.ToLower()))
            return BadRequest("Stock already in portfolio");

        var portfolioModel = new Portfolio
        {
            StockId = stock.Id,
            AppUserId = appUser.Id
        };

        await _portfolioRepository.CreatePortfolio(portfolioModel);

        if (portfolioModel is null)
            return StatusCode(500, "Could not create");
        else
            return Created();
    }
    
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeletePortfolio(string symbol)
    {
        var userName = User.GetUserName();
        var appUser = await _userManager.FindByNameAsync(userName);
        
        var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

        var filteredStock = userPortfolio
            .Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();
        
        if (filteredStock.Count == 1)
            await _portfolioRepository.DeletePortfolio(appUser, symbol);
        else 
            return BadRequest("Stock not found in portfolio");

        return Ok();
    }
}