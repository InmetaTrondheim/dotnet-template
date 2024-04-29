using AutoMapper;
using AutoMapper.QueryableExtensions;
using InmetaTemplate.Application.Common.Interfaces;
using InmetaTemplate.Application.TodoItems.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InmetaTemplate.Application.TodoItems.Queries;

public record GetTodoItemsQuery : IRequest<IEnumerable<TodoItemDto>>;

public class GetTodoItemsQueryHandler : IRequestHandler<GetTodoItemsQuery, IEnumerable<TodoItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTodoItemsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TodoItemDto>> Handle(GetTodoItemsQuery request, CancellationToken cancellationToken)
    {
        return await _context.TodoItems
            .AsNoTracking()
            .ProjectTo<TodoItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}