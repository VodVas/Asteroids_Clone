using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
    public sealed class CollisionDetector : ICollisionDetector
    {
        private readonly GameConfig _config;
        private readonly GameState _gameState;
        private readonly EntityRegistry _entityManager;
        private readonly EntityFactory _spawnService;
        private readonly Player _player;

        public CollisionDetector(GameConfig config, GameState gameState, EntityRegistry entityManager,
            EntityFactory spawnService, Player player)
        {
            _config = config;
            _gameState = gameState;
            _entityManager = entityManager;
            _spawnService = spawnService;
            _player = player;
        }

        public void CheckCollisions()
        {
            if (_gameState.IsGameOver) return;

            var entities = _entityManager.Entities;

            if (_player.IsAlive)
            {
                foreach (var entity in entities)
                {
                    if (!entity.IsActive) continue;

                    if (entity.Type == EntityType.Asteroid || entity.Type == EntityType.Ufo)
                    {
                        if (CheckCollision(_player.Position, entity.Position, GetCollisionRadius(entity)))
                        {
                            _player.Kill();
                            _gameState.GameOver();
                            return;
                        }
                    }
                }
            }

            var bullets = new List<Bullet>();

            foreach (var entity in entities)
            {
                if (entity is Bullet bullet && bullet.IsActive)
                    bullets.Add(bullet);
            }

            foreach (var bullet in bullets)
            {
                foreach (var entity in entities)
                {
                    if (!entity.IsActive || entity == bullet) continue;

                    if (entity.Type == EntityType.Asteroid || entity.Type == EntityType.Ufo)
                    {
                        if (CheckCollision(bullet.Position, entity.Position, GetCollisionRadius(entity)))
                        {
                            HandleBulletHit(bullet, entity);
                            break;
                        }
                    }
                }
            }
        }

        public void HandleLaserFire(Vector2 origin, Vector2 direction)
        {
            foreach (var entity in _entityManager.Entities)
            {
                if (!entity.IsActive) continue;

                if (entity.Type == EntityType.Asteroid || entity.Type == EntityType.Ufo)
                {
                    if (CheckLaserHit(origin, direction, entity.Position, GetCollisionRadius(entity)))
                    {
                        HandleLaserHit(entity);
                    }
                }
            }
        }

        private void HandleBulletHit(Bullet bullet, IGameEntity target)
        {
            bullet.Destroy();
            _entityManager.RemoveEntity(bullet);

            if (target is Asteroid asteroid)
            {
                HandleAsteroidDestruction(asteroid);
            }
            else if (target is Ufo ufo)
            {
                ufo.Destroy();
                _entityManager.RemoveEntity(ufo);
                _gameState.AddScore(_config.UfoScore);
            }
        }

        private void HandleLaserHit(IGameEntity target)
        {
            if (target is Asteroid asteroid)
            {
                HandleAsteroidDestruction(asteroid);
            }
            else if (target is Ufo ufo)
            {
                ufo.Destroy();
                _entityManager.RemoveEntity(ufo);
                _gameState.AddScore(_config.UfoScore);
            }
        }

        private void HandleAsteroidDestruction(Asteroid asteroid)
        {
            asteroid.Destroy();
            _entityManager.RemoveEntity(asteroid);

            _gameState.AddScore(_config.AsteroidScores[3 - asteroid.Size]);

            if (asteroid.Size > 1)
            {
                for (int i = 0; i < _config.AsteroidFragments; i++)
                {
                    var angle = i * (360f / _config.AsteroidFragments) * Mathf.Deg2Rad;
                    var offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * _config.AsteroidFragmentOffsetDistance;
                    _spawnService.SpawnAsteroid(asteroid.Position + offset, asteroid.Size - 1);
                }
            }
        }

        private bool CheckCollision(Vector2 pos1, Vector2 pos2, float radius)
        {
            return Vector2.Distance(pos1, pos2) < radius;
        }

        private bool CheckLaserHit(Vector2 origin, Vector2 direction, Vector2 targetPos, float targetRadius)
        {
            var toTarget = targetPos - origin;
            var distance = Vector2.Dot(toTarget, direction);

            if (distance < 0 || distance > _config.LaserRange) return false;

            var closest = origin + direction * distance;
            return Vector2.Distance(closest, targetPos) < targetRadius;
        }

        private float GetCollisionRadius(IGameEntity entity)
        {
            return entity switch
            {
                Asteroid asteroid => asteroid.Size * _config.AsteroidColliderRadiusPerSize,
                Ufo => _config.UfoColliderRadius,
                _ => _config.DefaultColliderRadius
            };
        }
    }
}