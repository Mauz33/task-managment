using System.Text.Json;
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
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    

    public TasksController(ApplicationContext context, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskModelCreate taskModelCreate)
    {
        var userId = User.Claims.First(x => x.Type == "userId").Value;
        var httpClient = _httpClientFactory.CreateClient();

        var jwt = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        
        httpClient.DefaultRequestHeaders.Add("Authorization", jwt);

        var response = await httpClient.GetAsync($"http://localhost:5269/Users/DoesExistsById?Id={userId}");
        var content = await response.Content.ReadAsStreamAsync();
        var exists = await JsonSerializer.DeserializeAsync<bool>(content);

        var task = new TaskModel()
        {
            Title = taskModelCreate.Title ?? string.Empty,
            Description = taskModelCreate.Description ?? string.Empty,
            CreatedById = int.Parse(userId),
            DateDeadline = taskModelCreate.DateDeadline,
            AssignToIds = taskModelCreate.AssignToIds, // TODO: check does id exist in user.service
            DateCreated = DateTime.UtcNow,
            TaskStatusId = taskModelCreate.StatusId
        };

        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    public record TaskModelCreate(
        List<int> AssignToIds,
        DateTime DateDeadline,
        string? Title,
        string? Description,
        TaskStatuses StatusId);

    
}