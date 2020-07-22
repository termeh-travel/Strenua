using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Termeh.Toolkit.Domain.Persistence;

namespace Termeh.Toolkit.EntityFramework
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public EfRepository(IDbContextAccessor contextAccessor)
        {
            Context = contextAccessor.Context;
            DbSet = Context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public async Task<TEntity> GetByIdAsync<TPrimaryKey>(TPrimaryKey primaryKey)
        {
            return await DbSet.FindAsync(primaryKey);
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }
    }
}