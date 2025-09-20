using UnityEngine;

namespace AsteroidsClone
{
    public sealed class Ufo : IGameEntity
    {
        private Vector2 _position;
        private Vector2 _velocity;
        private bool _isActive;

        public int Id { get; }
        public Vector2 Position => _position;
        public Vector2 Velocity => _velocity;
        public float Rotation => 0f;
        public bool IsActive => _isActive;
        public EntityType Type => EntityType.Ufo;

        public Ufo(int id, Vector2 position)
        {
            Id = id;
            _position = position;
            _velocity = Vector2.zero;
            _isActive = true;
        }

        public void UpdateTarget(Vector2 targetPosition, float speed)
        {
            if (!_isActive) return;

            var direction = (targetPosition - _position).normalized;
            _velocity = direction * speed;
        }

        public void Update(float deltaTime, GameConfig config)
        {
            if (!_isActive) return;

            _position += _velocity * deltaTime;

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