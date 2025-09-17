using BM.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BM.Data.Repositories
{
    public class Repository<TEntity>(DataContext context) : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context =
            context ?? throw new ArgumentNullException(nameof(context));

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public Task<TEntity> Update(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
            return Task.FromResult(entity);
        }
    }
}
