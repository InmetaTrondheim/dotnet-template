using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template._1.Application.Common.Interfaces;
using Template._1.Application.TodoItems.Dtos;
using Template._1.Domain.Entities;
using Template._1.Domain.ErrorHandling;

namespace Template._1.Application.TodoItems.Queries;

public record GetTodoItemQuery(Guid Id) : IRequest<TodoItemDto>;

public class GetTodoItemQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetTodoItemQuery, TodoItemDto>
{
    public async Task<TodoItemDto> Handle(GetTodoItemQuery request, CancellationToken cancellationToken)
    {
        var result = await context.TodoItems
            .AsNoTracking()
            .ProjectTo<TodoItemDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (result == null)
            throw new ApiException(CommonErrors.EntityNotFound<TodoItem>(request.Id));

        return result;
    }
}