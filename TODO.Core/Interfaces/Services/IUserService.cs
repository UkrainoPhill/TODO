namespace TODO.Core.Interfaces.Services;

public interface IUserService
{
    void Register(string username, string password, string email);
    string Login(string emailOrUsername, string password);
}