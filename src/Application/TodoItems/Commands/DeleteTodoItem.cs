using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.TodoItems.Commands;

public record DeleteTodoItemCommand(int Id) : IRequest;

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new Exception(); //TODO: NotFound exception with handling
        }

        entity.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);
    }
}