using AutoMapper;
using AutoMapper.QueryableExtensions;
using InmetaTemplate.Application.Common.Interfaces;
using InmetaTemplate.Application.TodoItems.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InmetaTemplate.Application.TodoItems.Queries;

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