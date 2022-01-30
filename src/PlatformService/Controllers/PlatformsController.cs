using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data.Repositories;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IRepository<Platform> _platformRepository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformsController(IRepository<Platform> platformRepository,
        IMapper mapper,
        ICommandDataClient commandDataClient)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> Get() 
        {
            var platforms = _platformRepository.GetAll();
            var platformsDTO = _mapper.Map<IEnumerable<PlatformReadDTO>>(platforms);
            return Ok(platformsDTO);
        }
        [HttpGet("{id}", Name = "GetPlatformById")]
        public async  Task<ActionResult<PlatformReadDTO>> GetPlatformById(int id)
        {
            var platform = await _platformRepository.GetById(id);

            if(platform == null)
                return NotFound();

            return Ok(_mapper.Map<PlatformReadDTO>(platform));
        }
        [HttpPost]
        public async Task<ActionResult<PlatformReadDTO>> CreatePlatform(PlatformCreateDTO platformDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }
            var newPlatform = _mapper.Map<Platform>(platformDTO);
            await _platformRepository.Create(newPlatform);
            bool saved = await _platformRepository.SaveChanges();

            if(!saved)
            {
                return BadRequest();
            }
            
            var responseDTO = _mapper.Map<PlatformReadDTO>(newPlatform);
            
            try{
                await _commandDataClient.SendPlatformToCommand(responseDTO);
            } catch (Exception e)
            {
                Console.WriteLine($"Error with the connection. {e.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { id = newPlatform.Id }, responseDTO);            
        }
    }
}