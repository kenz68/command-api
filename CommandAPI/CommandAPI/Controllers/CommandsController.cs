using AutoMapper;
using CommandAPI.Data;
using CommandAPI.Dtos;
using CommandAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CommandAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommandsController: ControllerBase
{
    private readonly ICommandApiRepo _repository;
    private readonly IMapper _mapper;
    public CommandsController(ICommandApiRepo repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
    {
        var commandItems = _repository.GetAllCommands();
        return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
    }

    [HttpGet("{id:int}", Name = "GetCommandById")]
    public ActionResult<CommandReadDto> GetCommandById(int id)
    {
        var commandItem = _repository.GetCommandById(id);
        if (commandItem == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<CommandReadDto>(commandItem));
    }

    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
    {
        var commandModel = _mapper.Map<Command>(commandCreateDto);
        _repository.CreateCommand(commandModel);
        _repository.SaveChange();

        var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

        return CreatedAtRoute(nameof(GetCommandById), new {Id = commandReadDto.Id}, commandReadDto);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
    {
        var commandModelFromRepo = _repository.GetCommandById(id);
        if (commandModelFromRepo == null)
        {
            return NotFound();
        }

        _mapper.Map(commandUpdateDto, commandModelFromRepo);
        _repository.UpdateCommand(commandModelFromRepo);

        _repository.SaveChange();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
    {
        var commandModelFromRepo = _repository.GetCommandById(id);
        if (commandModelFromRepo == null)
        {
            return NotFound();
        }

        var commandToPath = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);
        patchDoc.ApplyTo(commandToPath, ModelState);
        
        if (!TryValidateModel(commandToPath))
        {
            return ValidationProblem();
        }

        _mapper.Map(commandToPath, commandModelFromRepo);
        _repository.UpdateCommand(commandModelFromRepo);
        _repository.SaveChange();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCommand(int id)
    {
        var commandModelFromRepo = _repository.GetCommandById(id);
        if (commandModelFromRepo == null)
        {
            return NotFound();
        }
        _repository.DeleteCommand(commandModelFromRepo);
        _repository.SaveChange();

        return NoContent();
    }
}