using api.DTOs.AccountDTO;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginDTO.UserName);
            
            if (user is null)
                return Unauthorized("Invalid username!");
            
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            
            if (result.Succeeded)
                return Ok(
                    new NewUserDTO
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        Token = _tokenService.CreateToken(user)
                    }
                );
            else
                return Unauthorized("Invalid username or password!");
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
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
                    return Ok(
                            new NewUserDTO
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)
                            }
                        );
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