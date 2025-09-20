using UnityEngine;

namespace AsteroidsClone
{
    public sealed class Asteroid : IGameEntity
    {
        private Vector2 _position;
        private Vector2 _velocity;
        private float _rotation;
        private float _rotationSpeed;
        private bool _isActive;
        private int _size;

        public int Id { get; }
        public Vector2 Position => _position;
        public Vector2 Velocity => _velocity;
        public float Rotation => _rotation;
        public bool IsActive => _isActive;
        public EntityType Type => EntityType.Asteroid;
        public int Size => _size;

        public Asteroid(int id, Vector2 position, Vector2 velocity, int size)
        {
            Id = id;
            _position = position;
            _velocity = velocity;
            _size = size;
            _rotation = Random.Range(0f, 360f);
            _rotationSpeed = Random.Range(-90f, 90f);
            _isActive = true;
        }

        public void Update(float deltaTime, GameConfig config)
        {
            if (!_isActive) return;

            _position += _velocity * deltaTime;
            _rotation += _rotationSpeed * deltaTime;

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