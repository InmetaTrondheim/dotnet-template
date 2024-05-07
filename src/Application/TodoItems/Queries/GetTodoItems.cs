using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template._1.Application.Common.Interfaces;
using Template._1.Application.TodoItems.Dtos;

namespace Template._1.Application.TodoItems.Queries;

public record GetTodoItemsQuery : IRequest<IEnumerable<TodoItemDto>>;

public class GetTodoItemsQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetTodoItemsQuery, IEnumerable<TodoItemDto>>
{
    public async Task<IEnumerable<TodoItemDto>> Handle(GetTodoItemsQuery request, CancellationToken cancellationToken)
    {
        return await context.TodoItems
            .AsNoTracking()
            .ProjectTo<TodoItemDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}