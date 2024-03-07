using api.DTOs.StockDTO;
using api.Models;
using AutoMapper;

namespace api.Mappers;

public class StockAutoMappers : Profile
{
    public StockAutoMappers()
    {
        CreateMap<Stock, StockDTO>();
        CreateMap<CreateStockRequestDTO, Stock>();
        CreateMap<UpdateStockRequestDTO, Stock>();
        CreateMap<FMPStock, Stock>();
    }
}