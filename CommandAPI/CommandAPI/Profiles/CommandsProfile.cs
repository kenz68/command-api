using CommandAPI.Dtos;
using CommandAPI.Models;

namespace CommandAPI.Profiles;
using AutoMapper;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        CreateMap<Command, CommandReadDto>();
    }
}