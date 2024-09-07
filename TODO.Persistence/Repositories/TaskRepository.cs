using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using TODO.Core.Enum;
using TODO.Core.Interfaces.Repositories;
using Task = TODO.Core.Models.Task;

namespace TODO.Persistence.Repositories;

public class TaskRepository (TodoContext context) : ITaskRepository
{
    public void AddTask(Task task)
    {
        context.Tasks.Add(task);
        context.SaveChanges();
    }

    public Task GetTask(Guid id)
    {
        var task = context.Tasks.FirstOrDefault(t => t.Id == id);
        return task ?? throw new InvalidOperationException();
    }

    public void DeleteTask(Task task)
    {
        context.Tasks.Remove(task);
        context.SaveChanges();
    }
    
    public void UpdateTask(Task newTask, Task task)
    {
        var existingTask = context.Tasks.Local.FirstOrDefault(t => t.Id == task.Id);
        if (existingTask == null) return;
        existingTask.Title = newTask.Title;
        existingTask.Description = newTask.Description;
        existingTask.DueDate = newTask.DueDate;
        existingTask.Status = newTask.Status;
        existingTask.Priority = newTask.Priority;
        existingTask.UpdatedAt = DateTime.UtcNow;
        context.Tasks.Update(existingTask);
        context.SaveChanges();
    }

    public IEnumerable<Task> GetTasks(Guid id, [Optional] int pageSize, [Optional] int page, [Optional] string sortBy, [Optional] int priority, [Optional] DateTime dueDate)
    {
        var tasks = context.Tasks.Where(t => t.UserId == id).ToList();
        if (dueDate != default)
        {
            tasks = tasks.Where(t => t.DueDate == dueDate).ToList();
        }
        if (priority != default)
        {
            tasks = tasks.Where(t => t.Priority == (Priority)priority).ToList();
        }
        if (sortBy is "Status" or "Priority" or "DueDate")
        {
            tasks = sortBy switch
            {
                "Status" => tasks.OrderBy(t => t.Status).ToList(),
                "Priority" => tasks.OrderBy(t => t.Priority).ToList(),
                "DueDate" => tasks.OrderBy(t => t.DueDate).ToList(),
                _ => tasks
            };
        }
        if (pageSize != default && page != default)
        {
            tasks = tasks.Skip(pageSize * (page - 1)).Take(pageSize).ToList();
        }
        return tasks;
    }
}