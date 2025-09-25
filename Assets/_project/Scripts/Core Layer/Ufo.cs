using UnityEngine;

namespace AsteroidsClone
{
    public sealed class Ufo : IGameEntity
    {
        private Vector2 _position;

        public Vector2 Position => _position;
        public EntityType Type => EntityType.Ufo;

        public Vector2 Velocity { get; private set; }
        public bool IsActive { get; private set; }
        public float Rotation { get; private set; } = 0f;
        public int Id { get; }

        public Ufo(int id, Vector2 position)
        {
            Id = id;
            _position = position;
            Velocity = Vector2.zero;
            IsActive = true;
        }

        public void UpdateTarget(Vector2 targetPosition, float speed)
        {
            if (!IsActive) return;

            var direction = (targetPosition - _position).normalized;
            Velocity = direction * speed;
        }

        public void Update(float deltaTime, GameConfig config)
        {
            if (!IsActive) return;

            _position += Velocity * deltaTime;

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