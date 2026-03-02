using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
 
namespace AuthService.Application.Services;
 
public class JwtTokenService(IConfiguration configuration) : IJwtTokenService
{
    public string GenerateToken(UserProfile userProfile)
{
    var jwtSettings = configuration.GetSection("JwtSettings");
    var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
    var issuer = jwtSettings["Issuer"] ?? "AuthDotnet";
    var audience = jwtSettings["Audience"] ?? "AuthDotnet";
    var expiryInMinutes = int.Parse(jwtSettings["ExpiryInMinutes"] ?? "30");
 
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
 
    var role = userProfile.User?.UserRoles?.FirstOrDefault()?.Role?.Name ?? "USER_ROLE";
 
    var claims = new List<Claim>
    {
        // FIX 2: Usar .ToString() evita el error del BinaryReader (CS1503)
        new Claim(JwtRegisteredClaimNames.Sub, userProfile.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
        new Claim(ClaimTypes.Role, role)
    };
 
    var token = new JwtSecurityToken(
        issuer: issuer,
        audience: audience,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
        signingCredentials: credentials
    );
 
    return new JwtSecurityTokenHandler().WriteToken(token);
}
 
}