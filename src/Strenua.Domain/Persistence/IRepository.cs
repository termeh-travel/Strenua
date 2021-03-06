﻿using System.Threading.Tasks;

namespace Strenua.Domain.Persistence
{
    public interface IRepository<TEntity> where TEntity: class
    {
        void Add(TEntity entity);
        Task<TEntity> GetByIdAsync<TPrimaryKey>(TPrimaryKey primaryKey);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}