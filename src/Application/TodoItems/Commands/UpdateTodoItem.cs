using AutoMapper;
using InmetaTemplate.Application.Common.Interfaces;
using InmetaTemplate.Application.TodoItems.Dtos;
using InmetaTemplate.Domain.Entities;
using InmetaTemplate.Domain.ErrorHandling;
using MediatR;

namespace InmetaTemplate.Application.TodoItems.Commands;

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
            throw new ApiException(CommonErrors.EntityNotFound<TodoItem>(request.Id));

        entity.Title = request.Dto.Title;
        entity.Description = request.Dto.Description;
        entity.Complete = request.Dto.Completed;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TodoItemDto>(entity);
    }
}