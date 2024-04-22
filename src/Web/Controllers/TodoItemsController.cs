using Application.TodoItems;
using Application.TodoItems.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class TodoItemsController : ControllerBase
{
    private readonly ISender _sender;

    public TodoItemsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TodoItemDto>), StatusCodes.Status200OK)]
    public async Task<IEnumerable<TodoItemDto>> Get()
    {
        return await _sender.Send(new GetTodoItemsQuery());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<TodoItemDto> Get(int id)
    {
        return await _sender.Send(new GetTodoItemQuery(id));
    }

    [HttpPost]
    [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
    public async Task<TodoItemDto> Create(CreateTodoItemCommand command)
    {
        return await _sender.Send(command);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task Update(UpdateTodoItemCommand command)
    {
        await _sender.Send(command);
    }
}