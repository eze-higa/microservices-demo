using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOs;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers 
{
    [ApiController]
    [Route("api/c/[controller]")]
    public class PlatformsController : ControllerBase 
    {
        private readonly ICommandRepository _commandRepository;
        private IMapper _mapper;

        public PlatformsController(ICommandRepository commandRepository, IMapper mapper)
        {
            _commandRepository = commandRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> GetPlatforms()
        {
            var platforms = _commandRepository.GetPlatforms();
            return Ok(_mapper.Map<IEnumerable<Platform>,IEnumerable<PlatformReadDTO>>(platforms));
        }
        
        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("testing connection");
            return Ok("Connection from Commands Controller");
        }
    }
}