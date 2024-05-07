using Microsoft.EntityFrameworkCore;
using Template._1.Domain.Entities;

namespace Template._1.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoItem> TodoItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}