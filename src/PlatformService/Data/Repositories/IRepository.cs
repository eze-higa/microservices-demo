namespace PlatformService.Data.Repositories
{
    public interface IRepository <T> where T : class
    {
        Task<bool> SaveChanges();
        
        ICollection<T> GetAll();
        Task<T?> GetById(int id);
        Task Create(T entity);
    }
    
}