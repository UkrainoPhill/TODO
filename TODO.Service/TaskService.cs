using System.Runtime.InteropServices;
using System.Transactions;
using Microsoft.Extensions.Logging;
using TODO.Core.Enum;
using TODO.Core.Interfaces.Infrastructure;
using TODO.Core.Interfaces.Repositories;
using TODO.Core.Interfaces.Services;
using Task = TODO.Core.Models.Task;

namespace TODO.Service;

public class TaskService(ITaskRepository _taskRepository, ITokenReader _tokenReader, IUserRepository _userRepository, ILogger<TaskService> _logger) : ITaskService
{
    public void CreateTask(string title, [Optional] string description, [Optional] DateTime dueDate,
        [Optional] int statusInt, [Optional] int priorityInt, string token)
    {
        using var scope = new TransactionScope();
        if (statusInt is >= 4 or < 1)
        {
            throw new ArgumentException("Invalid status parameter.");
        }
        if (priorityInt is >= 4 or < 1)
        {
            throw new ArgumentException("Invalid priority parameter.");
        }
        var status = (Status)statusInt;
        var priority = (Priority)priorityInt;
        var username = _tokenReader.GetClaimsFromToken(token);
        var user = _userRepository.GetUserByEmailOrUsername(username);
        var userId = user.Id;
        var task = Task.CreateTask(title, description, dueDate, status, priority, userId);
        _taskRepository.AddTask(task);
        scope.Complete();
        _logger.LogInformation("Task created: {TaskId} by {Username}", task.Id, username);
    }

    public Task GetTask(Guid id, string token)
    {
        using var scope = new TransactionScope();
        var task = _taskRepository.GetTask(id);
        var username = _tokenReader.GetClaimsFromToken(token);
        var user = _userRepository.GetUserById(task.UserId);
        if (user.Username != username)
        {
            throw new UnauthorizedAccessException("You are not authorized to view this task.");
        }
        scope.Complete();
        _logger.LogInformation("Task retrieved: {TaskId} by {Username}", task.Id, username);
        return task;
    }

    public void DeleteTask(Guid id, string token)
    {
        using var scope = new TransactionScope();
        var task = _taskRepository.GetTask(id);
        var username = _tokenReader.GetClaimsFromToken(token);
        var user = _userRepository.GetUserById(task.UserId);
        if (user.Username != username)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this task.");
        }
        _taskRepository.DeleteTask(task);
        scope.Complete();
        _logger.LogInformation("Task deleted: {TaskId} by {Username}", task.Id, username);
    }

    public void UpdateTask(Guid id, string title, string description, DateTime dueDate,
        int status, int priority, string token)
    {
        using var scope = new TransactionScope();
        if (status is >= 4 or < 1)
        {
            throw new ArgumentException("Invalid status parameter.");
        }
        if (priority is >= 4 or < 1)
        {
            throw new ArgumentException("Invalid priority parameter.");
        }
        var task = _taskRepository.GetTask(id);
        var username = _tokenReader.GetClaimsFromToken(token);
        var user = _userRepository.GetUserById(task.UserId);
        if (user.Username != username)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this task.");
        }
        var statusEnum = (Status)status;
        var priorityEnum = (Priority)priority;
        var newTask = Task.CreateTask(title, description, dueDate, statusEnum, priorityEnum, user.Id);
        newTask.UpdatedAt = DateTime.UtcNow;
        newTask.Id = task.Id;
        _taskRepository.UpdateTask(newTask, task);
        scope.Complete();
        _logger.LogInformation("Task updated: {TaskId} by {Username}", task.Id, username);
    }
    
    public IEnumerable<Task> GetTasks(string token, [Optional] int pageSize, [Optional] int page, [Optional] string sortBy, [Optional] int priority, [Optional] DateTime dueDate)
    {
        using var scope = new TransactionScope();
        var username = _tokenReader.GetClaimsFromToken(token);
        var user = _userRepository.GetUserByEmailOrUsername(username);
        if (user.Username != username)
        {
            throw new UnauthorizedAccessException("You are not authorized to view tasks.");
        }
        if (!string.IsNullOrWhiteSpace(sortBy) && (sortBy != "Status" && sortBy != "Priority" && sortBy != "DueDate"))
        {
            throw new ArgumentException("Invalid sort parameter.");
        }
        var tasks = _taskRepository.GetTasks(user.Id, pageSize, page, sortBy, priority, dueDate);
        scope.Complete();
        _logger.LogInformation("Tasks retrieved by {Username}", username);
        return tasks;
    }
}