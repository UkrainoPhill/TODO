using Microsoft.Extensions.Logging;
using Moq;
using TODO.Core.Interfaces.Infrastructure;
using TODO.Core.Interfaces.Repositories;
using TODO.Core.Interfaces.Services;
using TODO.Core.Models;
using TODO.Service;
using Xunit;
using ILogger = Castle.Core.Logging.ILogger;

namespace TODO.Tests.ServiceTest;

public class UserServiceTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly Mock<ILogger<UserService>> _loggerMock;
    private readonly IUserService _userService;

    public UserServiceTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _jwtProviderMock = new Mock<IJwtProvider>();
        _loggerMock = new Mock<ILogger<UserService>>();
        _userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object, _jwtProviderMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void Register_ShouldAddUser_WhenPasswordIsValid()
    {
        var username = "testuser";
        var password = "Valid123";
        var email = "test@example.com";
        var passwordHash = "hashedPassword";
        _passwordHasherMock.Setup(ph => ph.HashPassword(password)).Returns(passwordHash);
        _userService.Register(username, password, email);
        _userRepositoryMock.Verify(ur => ur.AddUser(It.Is<User>(u => u.Email == email && u.Username == username && u.PasswordHash == passwordHash)), Times.Once);
    }

    [Fact]
    public void Register_ShouldThrowArgumentException_WhenPasswordIsInvalid()
    {
        var username = "testuser";
        var password = "invalid";
        var email = "test@example.com";
        Assert.Throws<ArgumentException>(() => _userService.Register(username, password, email));
    }

    [Fact]
    public void Login_ShouldReturnToken_WhenCredentialsAreValid()
    {
        var emailOrUsername = "test@example.com";
        var password = "Valid123";
        var passwordHash = "hashedPassword";
        var user = User.CreateUser(emailOrUsername, "testuser", passwordHash);
        var token = "jwtToken";
        _userRepositoryMock.Setup(ur => ur.GetUserByEmailOrUsername(emailOrUsername)).Returns(user);
        _passwordHasherMock.Setup(ph => ph.VerifyHashedPassword(password, passwordHash)).Returns(true);
        _jwtProviderMock.Setup(jp => jp.GenerateToken(user)).Returns(token);
        var result = _userService.Login(emailOrUsername, password);
        Assert.Equal(token, result);
    }

    [Fact]
    public void Login_ShouldThrowException_WhenUserNotFound()
    {
        var emailOrUsername = "test@example.com";
        var password = "Valid123";
        _userRepositoryMock.Setup(ur => ur.GetUserByEmailOrUsername(emailOrUsername)).Returns((User)null);
        Assert.Throws<Exception>(() => _userService.Login(emailOrUsername, password));
    }

    [Fact]
    public void Login_ShouldThrowException_WhenPasswordIsInvalid()
    {
        var emailOrUsername = "test@example.com";
        var password = "Valid123";
        var passwordHash = "hashedPassword";
        var user = User.CreateUser(emailOrUsername, "testuser", passwordHash);
        _userRepositoryMock.Setup(ur => ur.GetUserByEmailOrUsername(emailOrUsername)).Returns(user);
        _passwordHasherMock.Setup(ph => ph.VerifyHashedPassword(password, passwordHash)).Returns(false);
        Assert.Throws<Exception>(() => _userService.Login(emailOrUsername, password));
    }
}