using System.ComponentModel.DataAnnotations;

namespace api.DTOs.AccountDTO;

public class LoginDTO
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}