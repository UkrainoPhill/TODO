using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TODO.Core.Interfaces.Infrastructure;

namespace TODO.Infrastructure;

public class TokenReader : ITokenReader
{
    private readonly string _secretKey;

    public TokenReader(IOptions<JwtOptions> jwtOptions)
    {
        _secretKey = jwtOptions.Value.SecretKey;
    }

    private ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = System.Text.Encoding.ASCII.GetBytes(_secretKey);


        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
        return principal;
    }

    public string GetClaimsFromToken(string token)
    {
        var principal = GetPrincipalFromToken(token);
        var username = principal.Claims.FirstOrDefault(claim => claim.Type == "username")?.Value;
        return username;
    }
}