using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
    public sealed class ObjectPoolManager
    {
        private readonly Dictionary<EntityType, Queue<GameObject>> _pools;
        private Dictionary<EntityType, GameObject> _prefabs;
        private GameConfig _config;

        public ObjectPoolManager()
        {
            _pools = new Dictionary<EntityType, Queue<GameObject>>
        {
            { EntityType.Asteroid, new Queue<GameObject>() },
            { EntityType.Bullet, new Queue<GameObject>() },
            { EntityType.Ufo, new Queue<GameObject>() }
        };
        }

        public void Initialize(GameConfig config, GameObject asteroidPrefab, GameObject bulletPrefab, GameObject ufoPrefab)
        {
            _config = config;
            _prefabs = new Dictionary<EntityType, GameObject>
        {
            { EntityType.Asteroid, asteroidPrefab },
            { EntityType.Bullet, bulletPrefab },
            { EntityType.Ufo, ufoPrefab }
        };
        }

        public void InitializePools()
        {
            InitializePool(EntityType.Asteroid, _config.AsteroidPoolInitial);
            InitializePool(EntityType.Bullet, _config.BulletPoolInitial);
            InitializePool(EntityType.Ufo, _config.UfoPoolInitial);
        }

        public GameObject GetFromPool(EntityType entityType)
        {
            var pool = _pools[entityType];

            if (pool.Count > 0)
            {
                return pool.Dequeue();
            }

            return Object.Instantiate(_prefabs[entityType]);
        }

        public void ReturnToPool(GameObject obj, EntityType entityType)
        {
            obj.SetActive(false);
            _pools[entityType].Enqueue(obj);
        }

        public void ClearPools()
        {
            foreach (var pool in _pools.Values)
            {
                while (pool.Count > 0)
                {
                    var obj = pool.Dequeue();

                    if (obj != null)
                        Object.Destroy(obj);
                }
            }
        }

        private void InitializePool(EntityType entityType, int count)
        {
            var pool = _pools[entityType];
            var prefab = _prefabs[entityType];

            if (prefab == null)
            {
                Debug.LogError($"Prefab for {entityType} is null!");
                return;
            }

            for (int i = 0; i < count; i++)
            {
                var obj = Object.Instantiate(prefab);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
        }
    }
}