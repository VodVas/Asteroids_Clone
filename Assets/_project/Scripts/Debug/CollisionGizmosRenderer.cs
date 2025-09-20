using UnityEngine;
using Zenject;

namespace AsteroidsClone
{
    public sealed class CollisionGizmosRenderer : MonoBehaviour
    {
        private GameController _gameController;
        private GameConfig _config;

        [Inject]
        public void Construct(GameController gameController, GameConfig config)
        {
            _gameController = gameController;
            _config = config;
        }

        private void OnDrawGizmos()
        {
            if (_gameController == null || _gameController.EntityManager == null) return;

            Gizmos.color = Color.red;

            foreach (var entity in _gameController.EntityManager.Entities)
            {
                if (!entity.IsActive) continue;

                if (entity is Asteroid asteroid)
                {
                    var radius = asteroid.Size * _config.AsteroidColliderRadiusPerSize;
                    Gizmos.DrawWireSphere(asteroid.Position, radius);
                }
            }

            if (_gameController.Player != null && _gameController.Player.IsAlive)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_gameController.Player.Position, _config.DefaultColliderRadius);
            }

            Gizmos.color = Color.blue;

            foreach (var entity in _gameController.EntityManager.Entities)
            {
                if (!entity.IsActive) continue;

                if (entity is Ufo)
                {
                    Gizmos.DrawWireSphere(entity.Position, _config.UfoColliderRadius);
                }
            }

            Gizmos.color = Color.yellow;

            foreach (var entity in _gameController.EntityManager.Entities)
            {
                if (!entity.IsActive) continue;

                if (entity is Bullet)
                {
                    Gizmos.DrawWireSphere(entity.Position, _config.DefaultColliderRadius);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_gameController == null || _gameController.EntityManager == null) return;

            foreach (var entity in _gameController.EntityManager.Entities)
            {
                if (!entity.IsActive) continue;

                if (entity is Asteroid asteroid)
                {
                    var radius = asteroid.Size * _config.AsteroidColliderRadiusPerSize;
                    var visualSize = asteroid.Size * _config.AsteroidVisualScaleFactor;

                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(asteroid.Position, radius);

                    Gizmos.color = Color.white;
                    Gizmos.DrawWireSphere(asteroid.Position, visualSize);

#if UNITY_EDITOR
                    UnityEditor.Handles.Label(asteroid.Position + Vector2.up * 0.5f,
                        $"Size: {asteroid.Size}\nCollider: {radius:F2}\nVisual: {visualSize:F2}");
#endif
                }
            }
        }
    }
}