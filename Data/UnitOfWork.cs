
namespace TwitterApi.Data
{
   public class UnitOfWork : IUnitOfWork
   {
      private readonly ApplicationDbContext _context;

      public UnitOfWork(ApplicationDbContext context)
      {
         _context = context;
      }

      public async Task<bool> CommityAsync()
      {
         return await _context.SaveChangesAsync() > 0;
      }

      public void Delete<TEntity>(TEntity entity)
         where TEntity : class
      {
         _context.Set<TEntity>().Remove(entity);
      }

      public IQueryable<TEntity> Get<TEntity>() 
         where TEntity : class
      {
         return _context.Set<TEntity>();
      }

      public async Task<TEntity> GetByIdAsync<TEntity>(object id)
         where TEntity: class
      {
         return await _context.Set<TEntity>().FindAsync(id) ??
            throw new EntryPointNotFoundException();
      }

      public async Task<TEntity> InsertAsync<TEntity>(TEntity entity)
         where TEntity : class
      {
         var result = await _context.Set<TEntity>().AddAsync(entity);
         return result.Entity;
      }
   }
}
