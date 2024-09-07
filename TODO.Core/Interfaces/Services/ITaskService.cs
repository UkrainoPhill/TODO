using System.Runtime.InteropServices;
using TODO.Core.Enum;
using Task = TODO.Core.Models.Task;

namespace TODO.Core.Interfaces.Services;

public interface ITaskService
{
    void CreateTask(string title, [Optional] string description, [Optional] DateTime dueDate,
        [Optional] int statusInt, [Optional] int priorityInt, string token);

    Task GetTask(Guid id, string token);
    void DeleteTask(Guid id, string token);
    void UpdateTask(Guid id, string title, string description, DateTime dueDate,
        int status, int priority, string token);
    IEnumerable<Task> GetTasks(string token, [Optional] int pageSize, [Optional] int page, [Optional] string sortBy, [Optional] int priority, [Optional] DateTime dueDate);
}