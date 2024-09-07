using TODO.Core.Models;

namespace TODO.Core.Interfaces.Repositories;

public interface IUserRepository
{
    void AddUser(User user);
    User GetUserByEmailOrUsername(string emailOrUsername);
    User GetUserById(Guid id);
}