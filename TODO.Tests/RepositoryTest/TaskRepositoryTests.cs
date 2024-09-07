using Microsoft.EntityFrameworkCore;
using TODO.Core.Enum;
using TODO.Core.Interfaces.Repositories;
using TODO.Core.Models;
using TODO.Persistence;
using TODO.Persistence.Repositories;
using Xunit;
using Task = TODO.Core.Models.Task;

namespace TODO.Tests.RepositoryTest;

public class TaskRepositoryTests
{
    private readonly TodoContext _context;
    private readonly ITaskRepository _taskRepository;

    public TaskRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new TodoContext(options);
        _taskRepository = new TaskRepository(_context);
    }

    [Fact]
    public void GetTasks_ShouldReturnCorrectPage()
    {
        var userId = Guid.NewGuid();
        var tasks = new List<Task>
        {
            Task.CreateTask("Task 1", "Description 1", DateTime.Now, Status.Pending, Priority.Low, userId),
            Task.CreateTask("Task 2", "Description 2", DateTime.Now, Status.Pending, Priority.Low, userId),
            Task.CreateTask("Task 3", "Description 3", DateTime.Now, Status.Pending, Priority.Low, userId)
        };

        _context.Tasks.AddRange(tasks);
        _context.SaveChanges();
        var result = _taskRepository.GetTasks(userId, 2, 1);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetTasks_ShouldFilterByPriority()
    {
        var userId = Guid.NewGuid();
        var tasks = new List<Task>
        {
            Task.CreateTask("Task 1", "Description 1", DateTime.Now, Status.Completed, Priority.High, userId),
            Task.CreateTask("Task 2", "Description 2", DateTime.Now, Status.Pending, Priority.Low, userId)
        };
        _context.Tasks.AddRange(tasks);
        _context.SaveChanges();
        var result = _taskRepository.GetTasks(userId, priority: (int)Priority.High);
        Assert.Single(result);
        Assert.Equal(Priority.High, result.First().Priority);
    }

    [Fact]
    public void GetTasks_ShouldSortByDueDate()
    {
        var userId = Guid.NewGuid();
        var tasks = new List<Task>
        {
            Task.CreateTask("Task 1", "Description 1", DateTime.Now.AddDays(1), Status.Pending, Priority.Low, userId),
            Task.CreateTask("Task 2", "Description 2", DateTime.Now.AddDays(2), Status.Pending, Priority.Low, userId)
        };
        _context.Tasks.AddRange(tasks);
        _context.SaveChanges();
        var result = _taskRepository.GetTasks(userId, sortBy: "dueDate");
        Assert.Equal(tasks.OrderBy(t => t.DueDate), result);
    }
    
    [Fact]
    public void AddTask_ShouldAddTask()
    {
        var task = Task.CreateTask("New Task", "Description", DateTime.Now, Status.Pending, Priority.Low, Guid.NewGuid());
        _taskRepository.AddTask(task);
        _context.SaveChanges();
        var addedTask = _context.Tasks.Find(task.Id);
        Assert.NotNull(addedTask);
        Assert.Equal(task.Title, addedTask.Title);
    }

    [Fact]
    public void GetTask_ShouldReturnTask()
    {
        var task = Task.CreateTask("New Task", "Description", DateTime.Now, Status.Pending, Priority.Low, Guid.NewGuid());
        _context.Tasks.Add(task);
        _context.SaveChanges();
        var retrievedTask = _taskRepository.GetTask(task.Id);
        Assert.NotNull(retrievedTask);
        Assert.Equal(task.Title, retrievedTask.Title);
    }

    [Fact]
    public void DeleteTask_ShouldRemoveTask()
    {
        var task = Task.CreateTask("New Task", "Description", DateTime.Now, Status.Pending, Priority.Low, Guid.NewGuid());
        _context.Tasks.Add(task);
        _context.SaveChanges();
        _taskRepository.DeleteTask(task);
        _context.SaveChanges();
        var deletedTask = _context.Tasks.Find(task.Id);
        Assert.Null(deletedTask);
    }

    [Fact]
    public void UpdateTask_ShouldModifyTask()
    {
        var task = Task.CreateTask("New Task", "Description", DateTime.Now, Status.Pending, Priority.Low, Guid.NewGuid());
        _context.Tasks.Add(task);
        _context.SaveChanges();
        var updatedTask = Task.CreateTask("Updated Task", "Updated Description", DateTime.Now, Status.Completed, Priority.High, task.UserId);
        _taskRepository.UpdateTask(updatedTask, task);
        _context.SaveChanges();
        var retrievedTask = _context.Tasks.Find(task.Id);
        Assert.NotNull(retrievedTask);
        Assert.Equal("Updated Task", retrievedTask.Title);
        Assert.Equal("Updated Description", retrievedTask.Description);
        Assert.Equal(Status.Completed, retrievedTask.Status);
        Assert.Equal(Priority.High, retrievedTask.Priority);
    }
}