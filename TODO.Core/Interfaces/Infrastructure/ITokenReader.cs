using System.Security.Claims;

namespace TODO.Core.Interfaces.Infrastructure;

public interface ITokenReader
{
    string GetClaimsFromToken(string token);
}