using Microsoft.EntityFrameworkCore;
using TODO.Core.Models;
using TODO.Core.Interfaces.Repositories;
using TODO.Persistence;
using TODO.Persistence.Repositories;
using Xunit;

namespace TODO.Tests.RepositoryTest;

public class UserRepositoryTest
{
    private readonly TodoContext _context;
    private readonly IUserRepository _userRepository;

    public UserRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new TodoContext(options);
        _userRepository = new UserRepository(_context);
    }

    [Fact]
    public void AddUser_ShouldAddUser()
    {
        var user = User.CreateUser("testuser", "test@example.com", "password123");
        _userRepository.AddUser(user);
        _context.SaveChanges();
        var addedUser = _context.Users.Find(user.Id);
        Assert.NotNull(addedUser);
        Assert.Equal(user.Email, addedUser.Email);
    }

    [Fact]
    public void GetUserByEmailOrUsername_ShouldReturnUser()
    {
        var user = User.CreateUser("testuser", "test@example.com", "password123");
        _context.Users.Add(user);
        _context.SaveChanges();
        var retrievedUser = _userRepository.GetUserByEmailOrUsername("test@example.com");
        Assert.NotNull(retrievedUser);
        Assert.Equal(user.Email, retrievedUser.Email);
    }

    [Fact]
    public void GetUserById_ShouldReturnUser()
    {
        var user = User.CreateUser("testuser", "test@example.com", "password123");
        _context.Users.Add(user);
        _context.SaveChanges();
        var retrievedUser = _userRepository.GetUserById(user.Id);
        Assert.NotNull(retrievedUser);
        Assert.Equal(user.Id, retrievedUser.Id);
    }
}