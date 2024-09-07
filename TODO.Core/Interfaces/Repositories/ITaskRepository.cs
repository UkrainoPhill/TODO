using System.Runtime.InteropServices;
using Task = TODO.Core.Models.Task;

namespace TODO.Core.Interfaces.Repositories;

public interface ITaskRepository
{
    void AddTask(Task task);
    Task GetTask(Guid id);
    void DeleteTask(Task task);
    void UpdateTask(Task newTask, Task task);
    IEnumerable<Task> GetTasks(Guid id, [Optional] int pageSize, [Optional] int page, [Optional] string sortBy, [Optional] int priority, [Optional] DateTime dueDate);
}