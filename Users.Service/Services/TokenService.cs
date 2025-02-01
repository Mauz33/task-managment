using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Users.Service.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;
    
    private readonly string? _secret;
    private readonly string? _accessLifeTime;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _secret = _configuration["JWT:SecretKey"];
        _accessLifeTime = _configuration["JWT:AccessTokenLifeTimeMinutes"];
        if (_secret is null || _accessLifeTime is null) throw new Exception("Token is not configured");
    }
    
    public string GenerateToken(int userId)
    {
        if (!double.TryParse(_accessLifeTime, out var lifeTime))
        {
            throw new Exception($"Cannot parse {nameof(_accessLifeTime)}");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret ?? throw new InvalidOperationException()));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var header = new JwtHeader(credentials);

        var claims = new List<Claim>() { new Claim(type: "userId", value: userId.ToString()) };
        var payload = new JwtPayload("", "", claims, null, DateTime.Now.AddMinutes(lifeTime));

        var token = new JwtSecurityToken(header, payload);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}