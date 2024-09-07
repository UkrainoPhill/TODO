using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TODO.Core.Interfaces.Infrastructure;
using TODO.Core.Models;

namespace TODO.Infrastructure;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _jwtOptions = options.Value;

    public string GenerateToken(User user)
    {
        Claim[] claims = [new("username", user.Username)];
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.Now.AddHours(_jwtOptions.ExpiresHours));
        var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenHandler;
    }
}