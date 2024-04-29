using InmetaTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InmetaTemplate.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoItem> TodoItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}