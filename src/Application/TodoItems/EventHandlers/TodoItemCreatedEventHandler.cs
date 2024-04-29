using InmetaTemplate.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InmetaTemplate.Application.TodoItems.EventHandlers;

public class TodoItemCreatedEventHandler : INotificationHandler<TodoItemCreatedEvent>
{
    private readonly ILogger<TodoItemCreatedEventHandler> _logger;

    public TodoItemCreatedEventHandler(ILogger<TodoItemCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TodoItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain event executed: {DomainEvent}", notification.GetType().Name);

        _logger.LogInformation("Todo item with title {title} created", notification.Item.Title);

        return Task.CompletedTask;
    }
}