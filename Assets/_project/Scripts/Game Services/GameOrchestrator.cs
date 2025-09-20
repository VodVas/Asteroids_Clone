using System;
using UnityEngine;

namespace AsteroidsClone
{
    public sealed class GameOrchestrator : IDisposable
    {
        private readonly GameState _gameState;
        private readonly GameConfig _config;
        private readonly IInputService _inputService;
        private readonly ISpawnService _spawnService;
        private readonly ICollisionDetector _collisionService;

        private readonly PlayerController _playerController;
        private readonly WeaponController _weaponController;
        private readonly EntityController _entityController;

        public GameState GameState => _gameState;
        public Player Player => _playerController.Player;
        public EntityRegistry EntityManager => _entityController.EntityManager;

        public event Action<Vector2, Vector2> OnLaserFired;

        public GameOrchestrator(GameConfig config, IInputService inputService, GameState gameState,
            PlayerController playerController, WeaponController weaponController, EntityController entityController,
            ISpawnService spawnService, ICollisionDetector collisionService)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            _gameState = gameState ?? throw new ArgumentNullException(nameof(gameState));
            _playerController = playerController ?? throw new ArgumentNullException(nameof(playerController));
            _weaponController = weaponController ?? throw new ArgumentNullException(nameof(weaponController));
            _entityController = entityController ?? throw new ArgumentNullException(nameof(entityController));
            _spawnService = spawnService ?? throw new ArgumentNullException(nameof(spawnService));
            _collisionService = collisionService ?? throw new ArgumentNullException(nameof(collisionService));

            _weaponController.OnLaserFired += (origin, direction) => OnLaserFired?.Invoke(origin, direction);
        }

        public void Initialize()
        {
            _gameState.Reset();
            _playerController.Initialize();
            _entityController.Initialize();
            _spawnService.Reset();

            SpawnInitialAsteroids();
        }

        public void Update(float deltaTime)
        {
            if (HandleRestart()) return;
            if (_gameState.IsGameOver) return;

            _playerController.Update(deltaTime);
            _weaponController.Update(deltaTime);
            _entityController.Update(deltaTime);

            _spawnService.Update(deltaTime);
            _collisionService.CheckCollisions();
        }

        private bool HandleRestart()
        {
            if (_inputService.RestartGame && _gameState.IsGameOver)
            {
                Initialize();
                return true;
            }

            return false;
        }

        private void SpawnInitialAsteroids()
        {
            for (int i = 0; i < _config.InitialAsteroidsCount; i++)
            {
                _spawnService.SpawnAsteroid();
            }
        }

        public void Dispose()
        {
            _playerController?.Dispose();
            _weaponController?.Dispose();
            _entityController?.Dispose();
        }
    }
}