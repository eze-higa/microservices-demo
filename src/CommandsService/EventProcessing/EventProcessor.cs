using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOs;
using CommandsService.Models;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }

        public async Task ProcessEvent(string message)
        {
            var evenType = DetermineEvent(message);
            switch (evenType)
            {
                case EventType.PlatformPublished:
                    await AddPlatform(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            var eventType = JsonSerializer.Deserialize<GenericEventDTO>(notificationMessage);

            switch (eventType.Event)
            {
                case "Platform_Published":
                    return EventType.PlatformPublished;
                default:
                    return EventType.Undertermined;                    

            }
        }
        private async Task AddPlatform(string platformPublishedMessage)
        {
            using(var scope = _serviceScopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<ICommandRepository>();
                var platformPublishedDTO = JsonSerializer.Deserialize<PlatformPublishedDTO>(platformPublishedMessage);

                try
                {
                    var platform = _mapper.Map<PlatformPublishedDTO,Platform>(platformPublishedDTO);

                    if (!repository.ExternalPlatformExists(platform.ExternalID))
                    {
                        await repository.CreatePlatform(platform);
                        await repository.SaveChangesAsync();
                    }                    
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Could not add a new Platform {e.Message}");
                }
            }
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undertermined
    }
}