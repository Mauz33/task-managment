using Microsoft.EntityFrameworkCore;
using Users.Service.Models;

namespace Users.Service.Database;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
}