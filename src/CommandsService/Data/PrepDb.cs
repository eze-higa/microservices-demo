using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data
{
    public static class PrepDb 
    {
        public static void PrepPopulation(WebApplication webApp)
        {
            using( var scope = webApp.Services.CreateScope())
            {
                var grpcClient = scope.ServiceProvider.GetService<IPlatformDataClient>();
                var platforms = grpcClient.ReturnAllPlatforms();
                SeedData(scope.ServiceProvider.GetService<ICommandRepository>(), platforms);
            }
        }

        private static void SeedData(ICommandRepository repository, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("--> Seeding new platforms");
            foreach(var platform in platforms)
            {
                if(!repository.ExternalPlatformExists(platform.ExternalID)){
                    repository.CreatePlatform(platform);
                }
            }
            repository.SaveChanges();
        }
    }
}