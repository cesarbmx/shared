﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using CesarBmx.Shared.Domain.ModelBuilders;
using CesarBmx.Shared.Domain.Models;

namespace CesarBmx.Shared.Persistence.Repositories
{
    public class LoggerRepository<TEntity>: IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly Repository<TEntity> _repository;
        private readonly Repository<AuditLog> _logRepository;

        public LoggerRepository(Repository<TEntity> repository, Repository<AuditLog> logRepository)
        {
            _repository = repository;
            _logRepository = logRepository;
        }

        public async Task<List<TEntity>> GetAll()
        {
            // Get all
            return await _repository.GetAll();
        }
        public async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            // Get all by expression
            return await _repository.GetAll(expression);
        }
        public async Task<TEntity> GetSingle(object id)
        {
            // Get by id
            return await _repository.GetSingle(id);
        }
        public async Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> expression)
        {
            // Get single by expression
            return await _repository.GetSingle(expression);
        }
        public void Add(TEntity entity)
        {
            // Add
            _repository.Add(entity);

            // Log
            _logRepository.Add(new AuditLog("Add", entity, entity.Id, entity.CreatedAt));
        }
        public void AddRange(List<TEntity> entities)
        {
            // Add
            foreach (var entity in entities)
            {
                Add(entity);
            }
        }
        public void Update(TEntity entity)
        {
            // Update
            _repository.Update(entity);

            // Log
            _logRepository.Add(new AuditLog("Update", entity, entity.Id, entity.CreatedAt));
        }
        public void UpdateRange(List<TEntity> entities)
        {
            // Update
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }
        public void Remove(TEntity entity)
        {
            // Remove
            _repository.Remove(entity);

            // Log
            _logRepository.Add(new AuditLog("Remove", entity, entity.Id, entity.CreatedAt));
        }
        public void RemoveRange(List<TEntity> entities)
        {
            // Remove
            foreach (var entity in entities)
            {
                Remove(entity);
            }
        }
        public void UpdateCollection(List<TEntity> currentEntities, List<TEntity> newEntities)
        {
            AddRange(EntityBuilder.BuildEntitiesToAdd(currentEntities, newEntities));
            UpdateRange(EntityBuilder.BuildEntitiesToUpdate(currentEntities, newEntities));
            RemoveRange(EntityBuilder.BuildEntitiesToRemove(currentEntities, newEntities));
        }
    }
}
