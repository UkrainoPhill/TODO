using TODO.Core.Interfaces.Repositories;
using TODO.Core.Models;

namespace TODO.Persistence.Repositories;

public class UserRepository(TodoContext context) : IUserRepository
{
    public void AddUser(User user)
    {
        context.Users.Add(user);
        context.SaveChanges();
    }
    
    public User GetUserByEmailOrUsername(string emailOrUsername)
    {
        return context.Users.FirstOrDefault(u => u.Email == emailOrUsername || u.Username == emailOrUsername);
    }
    
    public User GetUserById(Guid id)
    {
        return context.Users.FirstOrDefault(u => u.Id == id);
    }
}