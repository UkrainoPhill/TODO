using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using TODO.Core.Interfaces.Infrastructure;

namespace TODO.Core.Models;

[Table("User")]
public class User
{
    [Key]
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<Task>? Tasks { get; set; }

    private User(Guid id, string username, string email, string passwordHash, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
    public static User CreateUser(string username, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(username) || username.Length < 3 || username.Length > 50)
        {
            throw new ArgumentException("Username must be between 3 and 50 characters.");
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be empty.");
        }


        
        var createdAt = DateTime.UtcNow;
        var updatedAt = createdAt;
        var id  = Guid.NewGuid();
        var user = new User(id, username, email, passwordHash, createdAt, updatedAt );

        return user;
    }
}