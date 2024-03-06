using api.DTOs.AccountDTO;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;

    public AccountController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var appUser = new AppUser
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email
            };
            
            var createUser = await _userManager.CreateAsync(appUser, registerDTO.Password);

            if (createUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (roleResult.Succeeded)
                    return Ok("User created successfully");
                else
                    return BadRequest(roleResult.Errors);
            }
            else
            {
                return StatusCode(500, createUser.Errors);
            }
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}