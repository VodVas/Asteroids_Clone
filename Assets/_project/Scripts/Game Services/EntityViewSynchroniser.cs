using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
    public sealed class EntityViewSynchronizer
    {
        private ObjectPoolManager _poolManager;
        private GameConfig _config;
        private readonly Dictionary<int, GameObject> _entityViews = new Dictionary<int, GameObject>();
        private readonly Dictionary<int, EntityType> _entityTypes = new Dictionary<int, EntityType>();
        private readonly HashSet<int> _activeEntityIds = new HashSet<int>();
        private readonly List<int> _toRemove = new List<int>();

        public void Initialize(ObjectPoolManager poolManager, GameConfig config)
        {
            _poolManager = poolManager;
            _config = config;
        }

        public void UpdateEntityViews(IReadOnlyList<IGameEntity> entities)
        {
            _activeEntityIds.Clear();

            foreach (var entity in entities)
            {
                if (!entity.IsActive) continue;

                _activeEntityIds.Add(entity.Id);

                if (!_entityViews.ContainsKey(entity.Id))
                {
                    CreateEntityView(entity);
                }

                if (_entityViews.TryGetValue(entity.Id, out var view))
                {
                    view.transform.position = entity.Position;
                    view.transform.rotation = Quaternion.Euler(0, 0, entity.Rotation);
                }
            }
        }

        public void CleanupDestroyedViews()
        {
            _toRemove.Clear();

            foreach (var kvp in _entityViews)
            {
                if (!_activeEntityIds.Contains(kvp.Key))
                {
                    _toRemove.Add(kvp.Key);
                }
            }

            foreach (var id in _toRemove)
            {
                if (_entityViews.TryGetValue(id, out var view))
                {
                    var entityType = _entityTypes[id];
                    _poolManager.ReturnToPool(view, entityType);
                    _entityViews.Remove(id);
                    _entityTypes.Remove(id);
                }
            }
        }

        public void ClearAllViews()
        {
            foreach (var view in _entityViews.Values)
            {
                view.SetActive(false);
            }
            _entityViews.Clear();
            _entityTypes.Clear();
            _activeEntityIds.Clear();
        }

        private void CreateEntityView(IGameEntity entity)
        {
            var view = _poolManager.GetFromPool(entity.Type);

            if (entity is Asteroid asteroid)
            {
                view.transform.localScale = Vector3.one * (asteroid.Size * _config.AsteroidVisualScaleFactor);
            }

            view.SetActive(true);
            _entityViews[entity.Id] = view;
            _entityTypes[entity.Id] = entity.Type;
        }
    }
}