using api.DTOs.StockDTO;
using api.Interfaces;
using api.Mappers;
using api.Models;
using AutoMapper;
using Newtonsoft.Json;

namespace api.Services;

public class FMPService : IFMPService
{
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public FMPService(IMapper mapper, HttpClient httpClient, IConfiguration configuration)
    {
        _mapper = mapper;
        _httpClient = httpClient;
        _configuration = configuration;
    }
    
    public async Task<Stock> FindStockBySymbolAsync(string symbol)
    {
        try
        {
            var result = await _httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_configuration["FMPKey"]}");
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
                var stock = tasks[0];
                if (stock != null)
                {
                    return stock.ToStockFromFMP();
                }
                return null;
            }
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}