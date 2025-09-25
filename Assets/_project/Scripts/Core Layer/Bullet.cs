using UnityEngine;

namespace AsteroidsClone
{
    public sealed class Bullet : IGameEntity
    {
        private Vector2 _position;
        private float _lifetime;

        public Vector2 Position => _position;
        public EntityType Type => EntityType.Bullet;

        public Vector2 Velocity { get; private set; }
        public float Rotation { get; private set; }
        public bool IsActive { get; private set; }
        public int Id { get; }

        public Bullet(int id, Vector2 position, Vector2 velocity, float rotation, float lifetime)
        {
            Id = id;
            _position = position;
            Velocity = velocity;
            Rotation = rotation;
            _lifetime = lifetime;
            IsActive = true;
        }

        public void Update(float deltaTime, GameConfig config)
        {
            if (!IsActive) return;

            _position += Velocity * deltaTime;
            _lifetime -= deltaTime;

            if (_lifetime <= 0)
            {
                IsActive = false;
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
            IsActive = false;
        }
    }
}