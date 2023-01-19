using Microsoft.EntityFrameworkCore;

namespace WebApiControllers;

public class ApplicationDbContext : DbContext
{
    public DbSet<Todo> Todos => Set<Todo>();
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}
