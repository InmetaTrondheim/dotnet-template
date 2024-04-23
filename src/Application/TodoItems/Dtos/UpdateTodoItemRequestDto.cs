using FluentValidation;

namespace Application.TodoItems.Dtos;

public class UpdateTodoItemRequestDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool Completed { get; set; }
}

public class UpdateTodoItemRequestDtoValidator : AbstractValidator<UpdateTodoItemRequestDto>
{
    public UpdateTodoItemRequestDtoValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(50)
            .NotEmpty();
    }
}