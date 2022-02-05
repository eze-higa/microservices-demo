using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
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
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(IRepository<Platform> platformRepository,
        IMapper mapper,
        ICommandDataClient commandDataClient,
        IMessageBusClient messageBusClient)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
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
            //Sending sync message
            try
            {
                await _commandDataClient.SendPlatformToCommand(responseDTO);
            } 
            catch (Exception e)
            {
                Console.WriteLine($"Error with the connection. {e.Message}");
            }
            //sending async message
            try
            {
                var busMessageRequest = _mapper.Map<PlatformReadDTO, PlatformPublishedDTO>(responseDTO);
                busMessageRequest.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(busMessageRequest);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> error sending message to Message bus {ex.Message}");
            }
            return CreatedAtRoute(nameof(GetPlatformById), new { id = newPlatform.Id }, responseDTO);            
        }
    }
}