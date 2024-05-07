using Microsoft.EntityFrameworkCore;
using Template._1.Application.Common.Interfaces;
using Template._1.Domain.Entities;

namespace Template._1.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>()
            .HasQueryFilter(x => !x.IsDeleted);
    }
}