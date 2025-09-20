using System;
using UnityEngine;

namespace AsteroidsClone
{
    public sealed class EntityFactory : ISpawnService
    {
        private readonly GameConfig _config;
        private readonly GameState _gameState;
        private readonly EntityRegistry _entityManager;
        private readonly System.Random _random = new System.Random();

        private float _asteroidSpawnTimer;
        private float _ufoSpawnTimer;
        private float _currentSpawnDelay;

        public EntityFactory(GameConfig config, GameState gameState, EntityRegistry entityManager)
        {
            _config = config;
            _gameState = gameState;
            _entityManager = entityManager;
            _currentSpawnDelay = config.InitialSpawnDelay;
        }

        public void Update(float deltaTime)
        {
            if (_gameState.IsGameOver) return;

            _asteroidSpawnTimer += deltaTime;
            _ufoSpawnTimer += deltaTime;

            if (_asteroidSpawnTimer >= _currentSpawnDelay)
            {
                SpawnAsteroid();
                _asteroidSpawnTimer = 0f;
                _currentSpawnDelay = Mathf.Max(_config.MinSpawnDelay, _currentSpawnDelay * _config.SpawnAcceleration);
            }

            if (_ufoSpawnTimer >= _currentSpawnDelay * _config.UfoSpawnDelayMultiplier)
            {
                SpawnUfo();
                _ufoSpawnTimer = 0f;
            }
        }

        public void SpawnAsteroid(Vector2? position = null, int size = 0)
        {
            var spawnPos = position ?? GetRandomEdgePosition();
            var asteroidSize = size == 0 ? _config.DefaultAsteroidSize : size;
            var velocity = GetRandomVelocity(_config.AsteroidSpeeds[3 - asteroidSize]);
            var asteroid = new Asteroid(_gameState.GetNextEntityId(), spawnPos, velocity, asteroidSize);
            _entityManager.AddEntity(asteroid);
        }

        public void SpawnUfo()
        {
            var position = GetRandomEdgePosition();
            var ufo = new Ufo(_gameState.GetNextEntityId(), position);
            _entityManager.AddEntity(ufo);
        }

        public void SpawnBullet(Vector2 position, Vector2 direction, Vector2 playerVelocity)
        {
            var velocity = direction * _config.BulletSpeed + playerVelocity * _config.BulletInheritVelocityFactor;
            var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + _config.VisualBulletRotationOffset;
            var bullet = new Bullet(_gameState.GetNextEntityId(), position, velocity, rotation, _config.BulletLifetime);
            _entityManager.AddEntity(bullet);
        }

        public void Reset()
        {
            _asteroidSpawnTimer = 0f;
            _ufoSpawnTimer = 0f;
            _currentSpawnDelay = _config.InitialSpawnDelay;
        }

        private Vector2 GetRandomEdgePosition()
        {
            var side = _random.Next(4);
            var halfWidth = _config.ScreenWidth / 2f;
            var halfHeight = _config.ScreenHeight / 2f;

            return side switch
            {
                0 => new Vector2(-halfWidth - _config.EdgeSpawnMargin, (float)(_random.NextDouble() * _config.ScreenHeight - halfHeight)),
                1 => new Vector2(halfWidth + _config.EdgeSpawnMargin, (float)(_random.NextDouble() * _config.ScreenHeight - halfHeight)),
                2 => new Vector2((float)(_random.NextDouble() * _config.ScreenWidth - halfWidth), -halfHeight - _config.EdgeSpawnMargin),
                _ => new Vector2((float)(_random.NextDouble() * _config.ScreenWidth - halfWidth), halfHeight + _config.EdgeSpawnMargin)
            };
        }

        private Vector2 GetRandomVelocity(float speed)
        {
            var angle = (float)(_random.NextDouble() * Math.PI * 2);
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
        }
    }
}