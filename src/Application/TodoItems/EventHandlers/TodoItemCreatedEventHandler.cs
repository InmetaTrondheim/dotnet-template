using InmetaTemplate.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InmetaTemplate.Application.TodoItems.EventHandlers;

public class TodoItemCreatedEventHandler(ILogger<TodoItemCreatedEventHandler> logger)
    : INotificationHandler<TodoItemCreatedEvent>
{
    public Task Handle(TodoItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain event executed: {DomainEvent}", notification.GetType().Name);

        logger.LogInformation("Todo item with title {title} created", notification.Item.Title);

        return Task.CompletedTask;
    }
}