using AutoMapper;
using CommandsService.DTOs;
using CommandsService.Models;

namespace CommandsService.AutomapperProfiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            CreateMap<CommandCreateDTO, Command>();
            CreateMap<Command, CommandReadDTO>();
            CreateMap<Platform, PlatformReadDTO>();

        }
    }
}
