using Template._1.Domain.Common;
using Template._1.Domain.Entities;

namespace Template._1.Domain.Events;

public class TodoItemCreatedEvent : BaseEvent
{
    public TodoItemCreatedEvent(TodoItem item)
    {
        Item = item;
    }

    public TodoItem Item { get; }
}