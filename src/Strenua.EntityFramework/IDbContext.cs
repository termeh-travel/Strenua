using Microsoft.EntityFrameworkCore;

namespace Strenua.EntityFramework
{
    public interface IDbContextAccessor
    {
        public DbContext Context { get; }
    }
    
    public class DbContextAccessor : IDbContextAccessor
    {
        public DbContextAccessor(DbContext context)
        {
            Context = context;
        }

        public DbContext Context { get; }
    }
}