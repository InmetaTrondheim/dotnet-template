using MediatR;
using Template._1.Application.Common.Interfaces;
using Template._1.Domain.Entities;
using Template._1.Domain.ErrorHandling;

namespace Template._1.Application.TodoItems.Commands;

public record DeleteTodoItemCommand(int Id) : IRequest;

public class DeleteTodoItemCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteTodoItemCommand>
{
    public async Task Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
            throw new ApiException(CommonErrors.EntityNotFound<TodoItem>(request.Id));

        entity.IsDeleted = true;

        await context.SaveChangesAsync(cancellationToken);
    }
}