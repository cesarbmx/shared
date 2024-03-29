﻿using System.Collections.Generic;
using System.Linq;
using CesarBmx.Shared.Domain.Models;

namespace CesarBmx.Shared.Domain.Builders
{
    public static class EntityBuilder
    {
        public static List<T> BuildEntitiesToAdd<T>(List<T> entities, List<T> newEntities) where T : class, IEntity<T>
        {
            // Add those not found in the list
            var entitiesToAdd = new List<T>();
            foreach (var newEntity in newEntities)
            {
                if (entities.FirstOrDefault(x=>x.Id == newEntity.Id) == null)
                    entitiesToAdd.Add(newEntity);
            }

            // Return
            return entitiesToAdd;
        }
        public static List<T> BuildEntitiesToUpdate<T>(List<T> entities, List<T> newEntities) where T : class, IEntity<T>
        {
            // Update those found in the list
            var entitiesToUpdate= new List<T>();
            foreach (var entity in entities)
            {
                var newEntity = newEntities.FirstOrDefault(x => x.Id == entity.Id);
                if (newEntity != null)
                    entity.Update(newEntity);
                entitiesToUpdate.Add(entity);
            }

            // Return
            return entitiesToUpdate;
        }
        public static List<T> BuildEntitiesToRemove<T>(List<T> entities, List<T> newEntities) where T : class, IEntity<T>
        {
            // Remove those no longer in the list
            var entitiesToRemove = new List<T>();
            foreach (var entity in entities)
            {
                if (newEntities.FirstOrDefault(x => x.Id == entity.Id) == null)
                    entitiesToRemove.Add(entity);
            }

            // Return
            return entitiesToRemove;
        }
    }
}
