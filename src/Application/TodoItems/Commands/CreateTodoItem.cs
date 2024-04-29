﻿using AutoMapper;
using FluentValidation;
using InmetaTemplate.Application.Common.Interfaces;
using InmetaTemplate.Application.TodoItems.Dtos;
using InmetaTemplate.Domain.Entities;
using InmetaTemplate.Domain.Events;
using MediatR;

namespace InmetaTemplate.Application.TodoItems.Commands;

public record CreateTodoItemCommand : IRequest<TodoItemDto>
{
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, TodoItemDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateTodoItemCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TodoItemDto> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoItem
        {
            Title = request.Title,
            Description = request.Description,
            Complete = false
        };

        entity.AddDomainEvent(new TodoItemCreatedEvent(entity));

        _context.TodoItems.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TodoItemDto>(entity);
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