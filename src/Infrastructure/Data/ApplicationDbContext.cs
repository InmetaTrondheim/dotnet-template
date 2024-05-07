using Microsoft.EntityFrameworkCore;
using Template._1.Application.Common.Interfaces;
using Template._1.Domain.Entities;

namespace Template._1.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>()
            .HasQueryFilter(x => !x.IsDeleted);
    }
}