using System.ComponentModel.DataAnnotations;

namespace api.DTOs.StockDTO;

public class UpdateStockRequestDTO
{
    [Required]
    [MaxLength(10, ErrorMessage = "Symbol cannot be over 10 over characters")]
    public string Symbol { get; set; } = string.Empty;
    [Required]
    [MaxLength(10, ErrorMessage = "Company name cannot be over 10 over characters")]
    public string CompanyName { get; set; } = string.Empty;
    [Required]
    [Range(1, 1000000000, ErrorMessage = "Purchase cannot be negative")]
    public decimal Purchase { get; set; }
    [Required]
    [Range(0.001, 100, ErrorMessage = "LastDiv cannot be negative")]
    public decimal LastDiv { get; set; }
    [Required]
    [MaxLength(10, ErrorMessage = "Industry cannot be over 10 over characters")]
    public string Industry { get; set; } = string.Empty;
    [Required]
    [Range(1, 5000000000, ErrorMessage = "MarketCap cannot be negative")]
    public long MarketCap { get; set; }
}