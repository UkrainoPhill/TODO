using System.Text.RegularExpressions;
using System.Transactions;
using Microsoft.Extensions.Logging;
using TODO.Core.Interfaces.Infrastructure;
using TODO.Core.Interfaces.Repositories;
using TODO.Core.Interfaces.Services;
using TODO.Core.Models;

namespace TODO.Service;

public class UserService(IUserRepository _userRepository, IPasswordHasher _passwordHasher, IJwtProvider _jwtProvider, ILogger<UserService> _logger) : IUserService
{
    public void Register(string username, string password, string email)
    {
        using var scope = new TransactionScope();
        var passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$";
        var regex = new Regex(passwordPattern);
        
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8 || password.Length > 16 || !regex.IsMatch(password))
        {
            throw new ArgumentException("Password is invalid.");
        }
        var passwordHash = _passwordHasher.HashPassword(password);

        var user = User.CreateUser(username, email, passwordHash);
        _userRepository.AddUser(user);
        scope.Complete();
        _logger.LogInformation("User {Username} registred", username);
    }
    public string Login(string emailOrUsername, string password)
    {
        var user = _userRepository.GetUserByEmailOrUsername(emailOrUsername);
        if (user == null)
        {
            throw new ArgumentException("User not found.");
        }
        if (!_passwordHasher.VerifyHashedPassword(password, user.PasswordHash))
        {
            throw new ArgumentException("Password is invalid.");
        }
        var token = _jwtProvider.GenerateToken(user);
        _logger.LogInformation("User logged in: {EmailOrUsername}", emailOrUsername);
        return token;
    }
}