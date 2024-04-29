using InmetaTemplate.Domain.Common;
using InmetaTemplate.Domain.Entities;

namespace InmetaTemplate.Domain.Events;

public class TodoItemCreatedEvent : BaseEvent
{
    public TodoItemCreatedEvent(TodoItem item)
    {
        Item = item;
    }

    public TodoItem Item { get; }
}