namespace TwitterApi.Data
{
   public interface IUnitOfWork
   {
      IQueryable<TEntity> Get<TEntity>() 
         where TEntity : class;
      Task<TEntity> GetByIdAsync<TEntity>(object id) 
         where TEntity : class;


      Task<TEntity> InsertAsync<TEntity>(TEntity entity)
         where TEntity : class;
      void Delete<TEntity>(TEntity entity)
         where TEntity : class;

      Task<bool> CommityAsync();
   }
}
