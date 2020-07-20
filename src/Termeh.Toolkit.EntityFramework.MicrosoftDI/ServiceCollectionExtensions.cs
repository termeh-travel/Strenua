using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Termeh.Toolkit.EntityFramework;
using Termeh.Toolkit.Domain;

namespace Termeh.Toolkit.EntityFramework.MicrosoftDI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkUnitOfWork(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUnitOfWork, EfUnitOfWork>();
            
            return serviceCollection;
        }

        public static IServiceCollection AddSqlServerDatabaseContext<TContext>(this IServiceCollection serviceCollection, string dbConnectionString) where TContext : DbContext
        {
            serviceCollection.AddDbContext<TContext>(options =>
                options.UseSqlServer(dbConnectionString));

            serviceCollection.AddScoped<IDbContextAccessor>(s =>
            {
                var dbContext = s.GetRequiredService<TContext>();
                return new DbContextAccessor(dbContext);
            });
            
            return serviceCollection;
        }
    }
}