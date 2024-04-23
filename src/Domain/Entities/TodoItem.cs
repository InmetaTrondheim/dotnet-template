using Domain.Common;

namespace Domain.Entities;

public class TodoItem : BaseEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool Complete { get; set; }
}