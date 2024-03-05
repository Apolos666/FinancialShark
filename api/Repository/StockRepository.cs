using api.Data;
using api.DTOs.StockDTO;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDBContext _context;
    private readonly IMapper _mapper;

    public StockRepository(ApplicationDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<List<Stock>> GetAllAsync()
    {
        return await _context.Stocks
            .Include(c => c.Comments)
            .ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await _context.Stocks
            .Include(c => c.Comments)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Stock> CreateAsync(Stock updateStock)
    {
        await _context.Stocks.AddAsync(updateStock);
        await _context.SaveChangesAsync();
        return updateStock;
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO updateStock)
    {
        var existingStock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
        
        if (existingStock is null)
            return null;
        
        _mapper.Map(updateStock, existingStock);
        
        await _context.SaveChangesAsync();
        return existingStock;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
        
        if (stockModel is null)
            return null;
        
        _context.Stocks.Remove(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<bool> StockExists(int id)
    {
        return await _context.Stocks.AnyAsync(s => s.Id == id);
    }
}