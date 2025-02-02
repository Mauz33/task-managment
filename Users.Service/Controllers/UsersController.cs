using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Users.Service.Database;
using Users.Service.Models;
using Users.Service.Services;

namespace Users.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ApplicationContext _context;
    private readonly TokenService _tokenService;

    public UsersController(ApplicationContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }
    
    
    [HttpPost("registration")]
    public async Task<IActionResult> Registration([FromBody] RegistrationDto registrationDto)
    {
        var login = registrationDto.Login;

        var doesLoginExist = _context.Users.Any(x => x.Login == login);

        if (doesLoginExist) 
            return BadRequest($"Login: '{login}' already exists");

        var user = new User
        {
            Login = login,
            Password = registrationDto.Password
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return Created((string)null!, user);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = _context.Users
            .FirstOrDefault(x => x.Login == loginDto.Login && x.Password == loginDto.Password);

        if (user is null)
            return Unauthorized("Incorrect login or password");

        var token = _tokenService.GenerateToken(user.Id);

        return Ok(token);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUser([FromQuery] int Id)
    {
        var user = _context.Users
            .FirstOrDefault(x => x.Id == Id);
        if (user is null)
        {
            return NotFound($"User with id {Id} not found");
        }

        return Ok(user);
    }
    
    [Authorize]
    [HttpGet("DoesExistsById")]
    public async Task<IActionResult> DoesExistsById([FromQuery] int Id)
    {
        var exists = _context.Users.Any(x => x.Id == Id);
        
        return Ok(exists);
    }
    
    [Authorize]
    [HttpGet("some")]
    public async Task<IActionResult> Some()
    {
        return Ok(12);
    }
    
    
}

public record RegistrationDto(string Login, string Password);
public record LoginDto(string Login, string Password);
