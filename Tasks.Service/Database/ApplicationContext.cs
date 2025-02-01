using Microsoft.EntityFrameworkCore;
using Tasks.Service.Models;
using TaskStatus = Tasks.Service.Models.TaskStatus;

namespace Tasks.Service.Database;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    
    public DbSet<TaskModel> Tasks { get; set; }
    public DbSet<TaskStatus> TaskStatus { get; set; }
}