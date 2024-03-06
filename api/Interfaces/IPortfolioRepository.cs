using api.Models;

namespace api.Interfaces;

public interface IPortfolioRepository
{
    public Task<List<Stock>> GetUserPortfolio(AppUser user);
    public Task<Portfolio> CreatePortfolio(Portfolio portfolio);
    public Task<Portfolio> DeletePortfolio(AppUser user, string symbol);
}