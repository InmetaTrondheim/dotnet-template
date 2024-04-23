using Application.TodoItems.Commands;
using Application.TodoItems.Dtos;
using Application.TodoItems.Queries;
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
    public async Task<IActionResult> Get()
    {
        return Ok(await _sender.Send(new GetTodoItemsQuery()));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await _sender.Send(new GetTodoItemQuery(id)));
    }

    [HttpPost]
    [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateTodoItemCommand command)
    {
        var result = await _sender.Send(command);

        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, UpdateTodoItemRequestDto dto)
    {
        return Ok(await _sender.Send(new UpdateTodoItemCommand{Id = id, Dto = dto}));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(int id)
    {
        await _sender.Send(new DeleteTodoItemCommand(id));

        return NoContent();
    }
}