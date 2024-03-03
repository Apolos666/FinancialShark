using api.DTOs.StockDTO;
using api.Models;

namespace api.Mappers;

public static class StockMappers
{
    public static StockDTO toStockDTO(this Stock stockModel)
    {
        return new StockDTO
        {
            Id = stockModel.Id,
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Purchase = stockModel.Purchase,
            LastDiv = stockModel.LastDiv,
            Industry = stockModel.Industry,
            MarketCap = stockModel.MarketCap
        };
    }
}