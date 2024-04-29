using InmetaTemplate.Application.Common.Interfaces;
using InmetaTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InmetaTemplate.Infrastructure.Data;

public class InmetaTemplateDbContext : DbContext, IApplicationDbContext
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public InmetaTemplateDbContext(DbContextOptions<InmetaTemplateDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>()
            .HasQueryFilter(x => !x.IsDeleted);
    }
}