using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasks.Service.Database;
using Tasks.Service.Models;

namespace Tasks.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    private readonly ApplicationContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TasksController(ApplicationContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskModelCreate taskModelCreate)
    {
        // var task = new TaskModel()
        // {
        //     
        // }
            
        return Ok();
    }
    
    public record TaskModelCreate(
        List<int> AssignToIds,
        DateTime DateDeadline,
        string? Title,
        string? Description,
        TaskStatuses Status);

    
}