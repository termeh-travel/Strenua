using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Strenua.Domain;
using Strenua.Domain.Persistence;

namespace Strenua.EntityFramework.MicrosoftDI
{
    public static class ServiceCollectionExtensions
    {
                public static IServiceCollection AddEntityFrameworkUnitOfWork(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUnitOfWork, EfUnitOfWork>();

            return serviceCollection;
        }

        public static IServiceCollection AddSqlServerDatabaseContext<TContext>(
            this IServiceCollection serviceCollection, string dbConnectionString) where TContext : DbContext
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

        public static IServiceCollection AddEntityFrameworkDefaultRepository(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            return serviceCollection;
        }

        public static IServiceCollection AddEntityRepositories(this IServiceCollection serviceCollection,
            params Assembly[] assemblies)
        {
            var allTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .ToList();

            var repositoryType = typeof(IRepository<>);

            var repositoryTypes = allTypes
                .Where(t => t.Implements(repositoryType) && t.IsInterface);

            foreach (var interfaceType in repositoryTypes)
            {
                var implementations = allTypes.Where(t => interfaceType.IsAssignableFrom(t)).ToList();

                if (implementations.Count() > 1)
                {
                    throw new Exception($"There are two implementations of '{interfaceType.Name}' type");
                }
                else if (!implementations.Any())
                {
                    throw new Exception($"There are no implementations of '{interfaceType.Name}' type");
                }

                var implementation = implementations.Single();

                serviceCollection.AddScoped(interfaceType, implementation);
            }

            serviceCollection.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            return serviceCollection;
        }

        private static bool Implements(this Type type, Type otherType)
        {
            var result = type.GetInterfaces().Any(i => i.GetGenericTypeDefinition() == otherType);

            return true;
        }
    }
}