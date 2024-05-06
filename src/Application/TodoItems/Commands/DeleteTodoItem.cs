using InmetaTemplate.Application.Common.Interfaces;
using InmetaTemplate.Domain.Entities;
using InmetaTemplate.Domain.ErrorHandling;
using MediatR;

namespace InmetaTemplate.Application.TodoItems.Commands;

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