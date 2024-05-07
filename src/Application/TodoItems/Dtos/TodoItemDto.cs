using AutoMapper;
using Template._1.Domain.Entities;

namespace Template._1.Application.TodoItems.Dtos;

public class TodoItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool Complete { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TodoItem, TodoItemDto>();
        }
    }
}