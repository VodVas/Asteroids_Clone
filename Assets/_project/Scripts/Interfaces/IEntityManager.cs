using System.Collections.Generic;

namespace AsteroidsClone
{
    public interface IEntityManager
    {
        IReadOnlyList<IGameEntity> Entities { get; }
        void AddEntity(IGameEntity entity);
        void RemoveEntity(IGameEntity entity);
        void ProcessChanges();
        void Clear();
        IEnumerable<T> GetEntitiesOfType<T>() where T : class, IGameEntity;
    }
}