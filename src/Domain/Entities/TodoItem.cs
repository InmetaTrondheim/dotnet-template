using Template._1.Domain.Common;

namespace Template._1.Domain.Entities;

public class TodoItem : BaseEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool Complete { get; set; }
}