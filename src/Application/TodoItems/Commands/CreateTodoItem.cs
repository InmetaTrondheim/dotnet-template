using AutoMapper;
using FluentValidation;
using InmetaTemplate.Application.Common.Interfaces;
using InmetaTemplate.Application.TodoItems.Dtos;
using InmetaTemplate.Domain.Entities;
using InmetaTemplate.Domain.Events;
using MediatR;

namespace InmetaTemplate.Application.TodoItems.Commands;

public record CreateTodoItemCommand(string Title, string Description) : IRequest<TodoItemDto>;

public class CreateTodoItemCommandHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<CreateTodoItemCommand, TodoItemDto>
{
    public async Task<TodoItemDto> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoItem
        {
            Title = request.Title,
            Description = request.Description,
            Complete = false
        };

        entity.AddDomainEvent(new TodoItemCreatedEvent(entity));

        context.TodoItems.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<TodoItemDto>(entity);
    }
}

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(50)
            .NotEmpty();
    }
}