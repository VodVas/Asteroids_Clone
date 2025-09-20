using System;
using UnityEngine;

namespace AsteroidsClone
{
    public sealed class WeaponController : IDisposable
    {
        private readonly Player _player;
        private readonly GameConfig _config;
        private readonly IInputService _inputService;
        private readonly EntityFactory _spawnService;
        private readonly CollisionDetector _collisionService;

        private float _bulletCooldown;

        public event Action<Vector2, Vector2> OnLaserFired;

        public WeaponController(Player player, GameConfig config, IInputService inputService,
            EntityFactory spawnService, CollisionDetector collisionService)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            _spawnService = spawnService ?? throw new ArgumentNullException(nameof(spawnService));
            _collisionService = collisionService ?? throw new ArgumentNullException(nameof(collisionService));
        }

        public void Update(float deltaTime)
        {
            if (!_player.IsAlive) return;

            UpdateBulletCooldown(deltaTime);
            HandleShooting();
        }

        private void UpdateBulletCooldown(float deltaTime)
        {
            if (_bulletCooldown > 0)
                _bulletCooldown -= deltaTime;
        }

        private void HandleShooting()
        {
            if (_inputService.FireBullet && _bulletCooldown <= 0)
            {
                FireBullet();
                _bulletCooldown = _config.BulletCooldown;
            }

            if (_inputService.FireLaser && _player.TryFireLaser())
            {
                FireLaser();
            }
        }

        private void FireBullet()
        {
            var direction = CalculateDirection();
            var position = _player.Position + direction * _config.BulletPositionOffset;
            _spawnService.SpawnBullet(position, direction, _player.Velocity);
        }

        private void FireLaser()
        {
            var direction = CalculateDirection();
            _collisionService.HandleLaserFire(_player.Position, direction);
            OnLaserFired?.Invoke(_player.Position, direction);
        }

        private Vector2 CalculateDirection()
        {
            var radians = _player.Rotation * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        }

        public void Dispose()
        {
        }
    }
}