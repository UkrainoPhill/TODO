using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TODO.API.Contracts.TaskContracts;
using TODO.Core.Interfaces.Services;
using TODO.Service;

namespace TODO.API.Controllers;

[ApiController]
[Route("tasks")]
public class TaskController(ITaskService _taskService) : ControllerBase
{
    /// <summary>
    /// This method is used to create a task, it requires the user to be authenticated.
    /// </summary>
    /// <param name="taskCreateInputDto">
    /// TaskCreateInputDto is a data transfer object that contains the following properties:
    /// title: string (3 - 200 characters)
    /// description: string (Optional)
    /// dueDate: DateTime (Optional)
    /// status: Enum (Pending = 0, InProgress = 1, Done = 2) (Optional)
    /// priority: Enum (Low = 0, Medium = 1, High = 2) (Optional)
    /// </param>
    /// <returns>
    /// Nothing
    /// </returns>
    [Authorize]
    [HttpPost]
    public ActionResult CreateTask(TaskCreateInputDto taskCreateInputDto)
    {
        try
        {
            var token = HttpContext.Request.Cookies["access_token"];
            _taskService.CreateTask(taskCreateInputDto.title, taskCreateInputDto.description,
                taskCreateInputDto.dueDate,
                taskCreateInputDto.status, taskCreateInputDto.priority, token);
            return Created();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// This method is used to get a task by id, it requires the user to be authenticated.
    /// </summary>
    /// <param name="id">
    /// Task id in format guid
    /// </param>
    /// <returns>
    /// Task by id
    /// </returns>
    [Authorize]
    [HttpGet("{id:guid}")]
    public ActionResult GetTask([FromRoute] Guid id)
    {
        try
        {
            var token = HttpContext.Request.Cookies["access_token"];
            var task = _taskService.GetTask(id, token);
            var result = new TaskGetOutputDto(task.Id, task.Title, task.Description, task.DueDate, task.Status, task.Priority,
                task.CreatedAt, task.UpdatedAt);
            return Ok(result);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized();
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// This method is used to get all tasks, it requires the user to be authenticated.
    /// U can use pagination, sorting, filtering by priority and due date.
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="page"></param>
    /// <param name="priority"></param>
    /// <param name="dueDate"></param>
    /// <param name="sortBy"></param>
    /// <returns>
    /// List of tasks
    /// </returns>
    [Authorize]
    [HttpGet]
    public  ActionResult GetTasks([FromQuery] int pageSize, [FromQuery] int page, [FromQuery] int priority, [FromQuery] DateTime dueDate, [FromQuery] string? sortBy)
    {
        try
        {
            var token = HttpContext.Request.Cookies["access_token"];
            var tasks = _taskService.GetTasks(token, pageSize, page, sortBy, priority, dueDate);
            var result = tasks.Select(task => new TaskGetOutputDto(task.Id ,task.Title, task.Description, task.DueDate, task.Status, task.Priority, task.CreatedAt, task.UpdatedAt)).ToList();
            return Ok(result);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    /// <summary>
    /// This method is used to update a task by id, it requires the user to be authenticated.
    /// </summary>
    /// <param name="taskUpdateInputDto">
    /// This is a data transfer object that contains the following properties:
    /// Title : string
    /// Description : string
    /// DueDate : DateTime
    /// Status : Enum (Pending = 0, InProgress = 1, Done = 2)
    /// Priority : Enum (Low = 0, Medium = 1, High = 2)
    /// </param>
    /// <param name="id">
    /// Task id in format guid
    /// </param>
    /// <returns>
    /// Nothing
    /// </returns>
    [Authorize]
    [HttpPut("{id:guid}")]
    public ActionResult UpdateTask([FromBody]TaskUpdateInputDto taskUpdateInputDto, [FromRoute] Guid id)
    {
        try
        {
            var token = HttpContext.Request.Cookies["access_token"];
            _taskService.UpdateTask(id, taskUpdateInputDto.Title, taskUpdateInputDto.Description,
                taskUpdateInputDto.DueDate, taskUpdateInputDto.Status, taskUpdateInputDto.Priority, token);
            return NoContent();
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    /// <summary>
    /// This method is used to delete a task by id, it requires the user to be authenticated.
    /// </summary>
    /// <param name="id">
    /// Task id in format guid
    /// </param>
    /// <returns>
    /// Nothing
    /// </returns>
    [Authorize]
    [HttpDelete("{id:guid}")]
    public ActionResult DeleteTask([FromRoute] Guid id)
    {
        try
        {
            var token = HttpContext.Request.Cookies["access_token"];
            _taskService.DeleteTask(id, token);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}