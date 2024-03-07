using api.Models;

namespace api.Interfaces;

public interface IFMPService
{
    public Task<Stock> FindStockBySymbolAsync(string symbol);
}