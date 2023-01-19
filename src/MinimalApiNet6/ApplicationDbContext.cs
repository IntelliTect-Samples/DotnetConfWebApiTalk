using Microsoft.EntityFrameworkCore;

namespace MinimalApiNet6;

public class ApplicationDbContext : DbContext
{
    public DbSet<Todo> Todos => Set<Todo>();
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}
