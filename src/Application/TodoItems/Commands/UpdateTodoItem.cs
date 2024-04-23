using Application.Common.Interfaces;
using Application.TodoItems.Dtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using System;

namespace Application.TodoItems.Commands;

public record UpdateTodoItemCommand : IRequest<TodoItemDto>
{
    public int Id { get; init; }
    public UpdateTodoItemRequestDto Dto { get; init; } = new();
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand, TodoItemDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateTodoItemCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TodoItemDto> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new Exception(); //TODO: NotFound exception with handling
        }

        entity.Title = request.Dto.Title;
        entity.Description = request.Dto.Description;
        entity.Complete = request.Dto.Completed;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TodoItemDto>(entity);
    }
}