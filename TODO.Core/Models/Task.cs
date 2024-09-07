using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using TODO.Core.Enum;

namespace TODO.Core.Models;

[Table("Task")]
public class Task
{
    [Key] public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public Status Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    [ForeignKey("User")] public Guid UserId { get; set; }
    public User User { get; set; }

    private Task(Guid id, string title, string description, DateTime dueDate, Status status, Priority priority,
        DateTime createdAt, DateTime updatedAt, Guid userId)
    {
        Id = id;
        Title = title;
        Description = description;
        DueDate = dueDate;
        Status = status;
        Priority = priority;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        UserId = userId;
    }

    public static Task CreateTask(string title, [Optional] string description, [Optional] DateTime dueDate,
        [Optional] Status status, [Optional] Priority priority, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length < 3 || title.Length > 200)
        {
            throw new ArgumentException("Title must be between 3 and 200 characters.");
        }

        var createdAt = DateTime.UtcNow;
        var updatedAt = createdAt;
        var id = Guid.NewGuid();


        return new Task(id, title, description, dueDate, status, priority, createdAt, updatedAt, userId);
    }
}