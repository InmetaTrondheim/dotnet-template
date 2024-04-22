using Application.Common.Interfaces;
using MediatR;
using System;

namespace Application.TodoItems.Requests;

public record UpdateTodoItemCommand : IRequest
{
    public int Id { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public bool Completed { get; init; }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new Exception(); //TODO: NotFound exception with handling
        }

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.Complete = request.Completed;

        await _context.SaveChangesAsync(cancellationToken);
    }
}