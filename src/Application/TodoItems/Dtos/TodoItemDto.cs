using AutoMapper;
using Domain.Entities;

namespace Application.TodoItems.Dtos;

public class TodoItemDto
{
    public int Id { get; set; }
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