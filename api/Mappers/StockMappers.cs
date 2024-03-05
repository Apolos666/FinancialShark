using api.DTOs.StockDTO;
using api.Models;

namespace api.Mappers;

public static class StockMappers
{
    public static StockDTO ToStockDTO(this Stock stockModel)
    {
        return new StockDTO
        {
            Id = stockModel.Id,
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Purchase = stockModel.Purchase,
            LastDiv = stockModel.LastDiv,
            Industry = stockModel.Industry,
            MarketCap = stockModel.MarketCap,
            Comments = stockModel.Comments.Select(c => c.toCommentDTO()).ToList()
        };
    }
    
    public static Stock ToStockFromCreateStockRequestDTO(this CreateStockRequestDTO createStockRequestDTO)
    {
        return new Stock
        {
            Symbol = createStockRequestDTO.Symbol,
            CompanyName = createStockRequestDTO.CompanyName,
            Purchase = createStockRequestDTO.Purchase,
            LastDiv = createStockRequestDTO.LastDiv,
            Industry = createStockRequestDTO.Industry,
            MarketCap = createStockRequestDTO.MarketCap
        };
    }
}