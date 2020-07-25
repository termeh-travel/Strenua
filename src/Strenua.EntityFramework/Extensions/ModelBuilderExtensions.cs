using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Strenua.EntityFramework.Extensions
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Finds the property named <paramref name="propertyName"/> on all entities and sets its saving behavior to <paramref name="saveBehavior"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="propertyName"></param>
        /// <param name="saveBehavior"></param>
        /// <returns></returns>
        public static ModelBuilder SetPropertySaveBehavior(this ModelBuilder builder, string propertyName,
            PropertySaveBehavior saveBehavior)
        {
            var properties = builder.Model
                .GetEntityTypes()
                .SelectMany(entityType => entityType.GetProperties())
                .Where(p => p.Name == propertyName);

            foreach (var property in properties)
            {
                property.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            }

            return builder;
        }

        public static void OnEntityCreate<TEntity>(this DbContext dbContext, Action<TEntity> callback)
        {
            dbContext.ChangeTracker.Tracked += (sender, args) =>
            {
                if (!args.FromQuery && args.Entry.State == EntityState.Added && args.Entry.Entity is TEntity entity)
                {
                    callback(entity);
                }
            };
        }

        public static void OnEntityUpdate<TEntity>(this DbContext dbContext, Action<TEntity> callback)
        {
            dbContext.ChangeTracker.StateChanged += (sender, args) =>
            {
                if (args.NewState == EntityState.Modified && args.Entry.Entity is TEntity entity)
                {
                    callback(entity);
                }
            };
        }
    }
}