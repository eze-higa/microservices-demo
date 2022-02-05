using CommandsService.Models;

namespace CommandsService.Data
{
    public class CommandRepository : ICommandRepository
    {
        private readonly AppDbContext _dbContext;

        public CommandRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateCommand(int platformId, Command command)
        {
            if(command == null)
                throw new ArgumentNullException(nameof(command));

            command.PlatformId = platformId;
            await _dbContext.Commands.AddAsync(command);
        }

        public async Task CreatePlatform(Platform platform)
        {
            if(platform == null)
                throw new ArgumentNullException(nameof(platform));

            await _dbContext.Platforms.AddAsync(platform);
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _dbContext
                    .Commands
                    .FirstOrDefault(c => c.Id == commandId && platformId == c.PlatformId);
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _dbContext
                    .Commands
                    .Where(c => c.PlatformId == platformId)
                    .ToList();
        }

        public IEnumerable<Platform> GetPlatforms()
        {
            return _dbContext.Platforms.ToList();
        }

        public bool PlatformExists(int platformId)
        {
            return _dbContext.Platforms.Any(p => p.Id == platformId);
        }

        public async Task<bool> SaveChanges()
        {
            var result = await _dbContext.SaveChangesAsync();
            return result >= 0;
        }
    }
}