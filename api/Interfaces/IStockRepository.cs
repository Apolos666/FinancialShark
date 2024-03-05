using api.DTOs.StockDTO;
using api.Models;

namespace api.Interfaces;

public interface IStockRepository
{
    public Task<List<Stock>> GetAllAsync();
    public Task<Stock?> GetByIdAsync(int id);
    public Task<Stock> CreateAsync(Stock updateStock);
    public Task<Stock?> UpdateAsync(int id ,UpdateStockRequestDTO updateStock);
    public Task<Stock?> DeleteAsync(int id);
    public Task<bool> StockExists(int id);
}