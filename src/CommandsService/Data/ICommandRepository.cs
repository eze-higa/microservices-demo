using CommandsService.Models;

namespace CommandsService.Data 
{
    public interface ICommandRepository 
    {
        public Task<bool> SaveChangesAsync();
        public bool SaveChanges();
        public IEnumerable<Platform> GetPlatforms();
        public Task CreatePlatform(Platform platform);
        public bool PlatformExists(int platformId);
        bool ExternalPlatformExists(int externalPlatformId);

        public IEnumerable<Command> GetCommandsForPlatform(int platformId);
        public Command GetCommand(int platformId, int commandId);
        public Task CreateCommand(int platformId, Command command);
    }
}