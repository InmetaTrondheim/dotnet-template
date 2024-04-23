using Application.Common.Interfaces;
using Application.TodoItems.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.TodoItems.Queries;

public record GetTodoItemQuery(int Id) : IRequest<TodoItemDto>;

public class GetTodoItemQueryHandler : IRequestHandler<GetTodoItemQuery, TodoItemDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTodoItemQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TodoItemDto> Handle(GetTodoItemQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.TodoItems
            .AsNoTracking()
            .ProjectTo<TodoItemDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (result == null)
        {
            throw new Exception(); //TODO: NotFound exception with handling
        }

        return result;
    }
}