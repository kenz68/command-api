using CommandAPI.Data;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommandsController: ControllerBase
{
    private readonly ICommandApiRepo _repository;

    public CommandsController(ICommandApiRepo repository)
    {
        _repository = repository;
    }
    [HttpGet]
    public ActionResult<IEnumerable<String>> GetAllCommands()
    {
        var commandItems = _repository.GetAllCommands();
        return Ok(commandItems);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Command> GetCommandById(int id)
    {
        var commandItem = _repository.GetCommandById(id);
        if (commandItem == null)
        {
            return NotFound();
        }

        return Ok(commandItem);
    }
}