using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOs;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/c/platforms/{platformId}/[controller]")]
    public class CommandsController : ControllerBase 
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepository commandRepository, IMapper mapper)
        {
            _commandRepository = commandRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDTO>> GetCommandsForPlatform(int platformId)
        {
            if(!_commandRepository.PlatformExists(platformId))
                return NotFound();

            var commands = _commandRepository.GetCommandsForPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<Command>,IEnumerable<CommandReadDTO>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform" )]
        public ActionResult<CommandReadDTO> GetCommandForPlatform(int platformId, int commandId)
        {
            if(!_commandRepository.PlatformExists(platformId))
                return NotFound();

            var command = _commandRepository.GetCommand(platformId, commandId);
            
            if(command == null)
                return NotFound();

            return Ok(_mapper.Map<Command, CommandReadDTO>(command));
        }

        [HttpPost]
        public async Task<ActionResult<CommandReadDTO>> CreateCommand(int platformId, CommandCreateDTO commandCreateDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest();    

            if(!_commandRepository.PlatformExists(platformId))
                return NotFound();
            
            var command = _mapper.Map<CommandCreateDTO, Command>(commandCreateDTO);
            
            await _commandRepository.CreateCommand(platformId, command);
            var succeed = await _commandRepository.SaveChanges();

            if(!succeed)
                return BadRequest();

            var commandReadDTO = _mapper.Map<Command, CommandReadDTO>(command);
            
            return CreatedAtRoute(
                    nameof(GetCommandsForPlatform), 
                    new { platformId = platformId, commandId = command.Id }, 
                    commandReadDTO);
        }

    }
}