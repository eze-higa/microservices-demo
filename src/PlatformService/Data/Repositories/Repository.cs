namespace PlatformService.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        public Repository(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task Create(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public ICollection<T> GetAll()
        {
            return _dbContext.Set<T>().ToList<T>();
        }

        public async Task<T?> GetById(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<bool> SaveChanges()
        {
            var result = await _dbContext.SaveChangesAsync();
            return result >= 0;
        }
    }

}