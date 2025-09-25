using UnityEngine;

namespace AsteroidsClone
{
    public sealed class Asteroid : IGameEntity
    {
        private float _rotationSpeed;
        private Vector2 _position;

        public Vector2 Position => _position;
        public EntityType Type => EntityType.Asteroid;

        public Vector2 Velocity { get; private set; }
        public float Rotation { get; private set; }
        public bool IsActive { get; private set; }
        public int Size { get; private set; }
        public int Id { get; }

        public Asteroid(int id, Vector2 position, Vector2 velocity, int size)
        {
            Id = id;
            _position = position;
            Velocity = velocity;
            Size = size;
            Rotation = Random.Range(0f, 360f);
            _rotationSpeed = Random.Range(-90f, 90f);
            IsActive = true;
        }

        public void Update(float deltaTime, GameConfig config)
        {
            if (!IsActive) return;

            _position += Velocity * deltaTime;
            Rotation += _rotationSpeed * deltaTime;

            var halfWidth = config.ScreenWidth / 2f;
            var halfHeight = config.ScreenHeight / 2f;

            if (Position.x > halfWidth) _position.x = -halfWidth;
            else if (Position.x < -halfWidth) _position.x = halfWidth;

            if (Position.y > halfHeight) _position.y = -halfHeight;
            else if (Position.y < -halfHeight) _position.y = halfHeight;
        }

        public void Destroy()
        {
            IsActive = false;
        }
    }
}