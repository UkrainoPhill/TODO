using TODO.Core.Models;

namespace TODO.Core.Interfaces.Infrastructure;

public interface IJwtProvider
{
    string GenerateToken(User user);
}