﻿using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CesarBmx.Shared.Domain.ModelBuilders;
using CesarBmx.Shared.Domain.Models;

namespace CesarBmx.Shared.Persistence.Repositories
{
    public class Repository<TEntity>: IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext dbContext)
        {
            _dbSet = dbContext.Set<TEntity>();
        }

        public async Task<List<TEntity>> GetAll()
        {
            // Get all
            return await _dbSet.ToListAsync();
        }
        public async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            // Get all by expression
            return await _dbSet.Where(expression).ToListAsync();
        }
        public async Task<TEntity> GetSingle(object id)
        {
            // Get by id
            return await _dbSet.FindAsync(id);
        }
        public async Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> expression)
        {
            // Get single by expression
            return await _dbSet.FirstOrDefaultAsync(expression);
        }
        public void Add(TEntity entity, DateTime time)
        {
            // Add
            _dbSet.Add(entity);
        }
        public void AddRange(List<TEntity> entities, DateTime time)
        {
            // Add
            foreach (var entity in entities)
            {
                Add(entity, time);
            }
        }
        public void Update(TEntity entity, DateTime time)
        {
            // Update
            _dbSet.Update(entity);
        }
        public void UpdateRange(List<TEntity> entities, DateTime time)
        {
            // Update
            foreach (var entity in entities)
            {
                Update(entity, time);
            }
        }
        public void Remove(TEntity entity, DateTime time)
        {
            // Remove
            _dbSet.Remove(entity);
        }
        public void RemoveRange(List<TEntity> entities, DateTime time)
        {
            // Remove
            foreach (var entity in entities)
            {
                Remove(entity, time);
            }
        }
        public void UpdateCollection(List<TEntity> currentEntities, List<TEntity> newEntities, DateTime time)
        {
            AddRange(EntityBuilder.BuildEntitiesToAdd(currentEntities, newEntities), time);
            UpdateRange(EntityBuilder.BuildEntitiesToUpdate(currentEntities, newEntities), time);
            RemoveRange(EntityBuilder.BuildEntitiesToRemove(currentEntities, newEntities), time);
        }
    }
}
