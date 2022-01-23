using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PreparationDb 
    {
        public static void PrepareDb(WebApplication webApp)
        {
            using(var scope = webApp.Services.CreateScope())
            {
                SeedData(scope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext dbContext)
        {
            if(!dbContext.Platforms.Any())
            {            
                dbContext.Platforms.AddRange(
                    new Platform { Name = "Dot Net", Publisher = "Microsoft", Cost = 0 },
                    new Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = 0 },
                    new Platform { Name = "MySQL", Publisher = "Oracle", Cost = 0 }
                );

                dbContext.SaveChanges();
                Console.WriteLine("Seeding data");                
            }
        }
    }
}