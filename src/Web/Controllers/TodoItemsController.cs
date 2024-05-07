using MediatR;
using Microsoft.AspNetCore.Mvc;
using Template._1.Application.TodoItems.Commands;
using Template._1.Application.TodoItems.Dtos;
using Template._1.Application.TodoItems.Queries;

namespace Template._1.Web.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class TodoItemsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TodoItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        return Ok(await sender.Send(new GetTodoItemsQuery()));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await sender.Send(new GetTodoItemQuery(id)));
    }

    [HttpPost]
    [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateTodoItemCommand command)
    {
        var result = await sender.Send(command);

        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, UpdateTodoItemRequestDto dto)
    {
        return Ok(await sender.Send(new UpdateTodoItemCommand(id, dto)));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(int id)
    {
        await sender.Send(new DeleteTodoItemCommand(id));

        return NoContent();
    }
}