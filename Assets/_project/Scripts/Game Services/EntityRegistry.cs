 using System.Collections.Generic;

namespace AsteroidsClone
{
    public sealed class EntityRegistry : IEntityManager
    {
        private readonly List<IGameEntity> _entities = new List<IGameEntity>();
        private readonly List<IGameEntity> _entitiesToAdd = new List<IGameEntity>();
        private readonly List<IGameEntity> _entitiesToRemove = new List<IGameEntity>();

        public IReadOnlyList<IGameEntity> Entities => _entities;

        public void AddEntity(IGameEntity entity)
        {
            _entitiesToAdd.Add(entity);
        }

        public void RemoveEntity(IGameEntity entity)
        {
            _entitiesToRemove.Add(entity);
        }

        public void ProcessChanges()
        {
            foreach (var entity in _entitiesToRemove)
            {
                _entities.Remove(entity);
            }
            _entitiesToRemove.Clear();

            foreach (var entity in _entitiesToAdd)
            {
                _entities.Add(entity);
            }
            _entitiesToAdd.Clear();
        }

        public void Clear()
        {
            _entities.Clear();
            _entitiesToAdd.Clear();
            _entitiesToRemove.Clear();
        }

        public IEnumerable<T> GetEntitiesOfType<T>() where T : class, IGameEntity
        {
            foreach (var entity in _entities)
            {
                if (entity is T typedEntity && entity.IsActive)
                {
                    yield return typedEntity;
                }
            }
        }
    }
}