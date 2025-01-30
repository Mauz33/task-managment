using Microsoft.EntityFrameworkCore;

namespace Users.Service.Database;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    
    
}