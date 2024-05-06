using AutoMapper;
using InmetaTemplate.Application.Common.Interfaces;
using InmetaTemplate.Application.TodoItems.Dtos;
using InmetaTemplate.Domain.Entities;
using InmetaTemplate.Domain.ErrorHandling;
using MediatR;

namespace InmetaTemplate.Application.TodoItems.Commands;

public record UpdateTodoItemCommand(int Id, UpdateTodoItemRequestDto Dto) : IRequest<TodoItemDto>;

public class UpdateTodoItemCommandHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<UpdateTodoItemCommand, TodoItemDto>
{
    public async Task<TodoItemDto> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
            throw new ApiException(CommonErrors.EntityNotFound<TodoItem>(request.Id));

        entity.Title = request.Dto.Title;
        entity.Description = request.Dto.Description;
        entity.Complete = request.Dto.Completed;

        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<TodoItemDto>(entity);
    }
}