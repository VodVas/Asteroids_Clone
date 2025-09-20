using UnityEngine;

namespace AsteroidsClone
{
    public sealed class Bullet : IGameEntity
    {
        private Vector2 _position;
        private Vector2 _velocity;
        private float _rotation;
        private float _lifetime;
        private bool _isActive;

        public int Id { get; }
        public Vector2 Position => _position;
        public Vector2 Velocity => _velocity;
        public float Rotation => _rotation;
        public bool IsActive => _isActive;
        public EntityType Type => EntityType.Bullet;

        public Bullet(int id, Vector2 position, Vector2 velocity, float rotation, float lifetime)
        {
            Id = id;
            _position = position;
            _velocity = velocity;
            _rotation = rotation;
            _lifetime = lifetime;
            _isActive = true;
        }

        public void Update(float deltaTime, GameConfig config)
        {
            if (!_isActive) return;

            _position += _velocity * deltaTime;
            _lifetime -= deltaTime;

            if (_lifetime <= 0)
            {
                _isActive = false;
                return;
            }

            var halfWidth = config.ScreenWidth / 2f;
            var halfHeight = config.ScreenHeight / 2f;

            if (_position.x > halfWidth) _position.x = -halfWidth;
            else if (_position.x < -halfWidth) _position.x = halfWidth;

            if (_position.y > halfHeight) _position.y = -halfHeight;
            else if (_position.y < -halfHeight) _position.y = halfHeight;
        }

        public void Destroy()
        {
            _isActive = false;
        }
    }
}