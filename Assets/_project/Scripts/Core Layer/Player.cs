using System;
using UnityEngine;

namespace AsteroidsClone
{
    public sealed class Player
    {
        private Vector2 _position;

        public event Action<Player> OnDestroyed;
        public event Action<int> OnLaserChargesChanged;

        public Vector2 Position => _position;
        public float Speed => Velocity.magnitude;

        public Vector2 Velocity { get; private set; }
        public float Rotation { get; private set; }
        public int LaserCharges { get; private set; }
        public float LaserCooldown { get; private set; }
        public bool IsThrusting { get; private set; }
        public bool IsAlive { get; private set; }


        public Player(GameConfig config)
        {
            Reset(config);
        }

        public void Reset(GameConfig config)
        {
            _position = Vector2.zero;
            Velocity = Vector2.zero;
            Rotation = 0f;
            LaserCharges = config.MaxLaserCharges;
            LaserCooldown = 0f;
            IsThrusting = false;
            IsAlive = true;
        }

        public void Rotate(float input, float deltaTime, GameConfig config)
        {
            Rotation += input * config.PlayerRotationSpeed * deltaTime;
            Rotation = (Rotation % 360f + 360f) % 360f;
        }

        public void Thrust(bool isThrusting, float deltaTime, GameConfig config)
        {
            IsThrusting = isThrusting;

            if (isThrusting)
            {
                var radians = Rotation * Mathf.Deg2Rad;
                var direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

                Velocity += direction * config.PlayerAcceleration * deltaTime;

                if (Velocity.magnitude > config.PlayerMaxSpeed)
                {
                    Velocity = Velocity.normalized * config.PlayerMaxSpeed;
                }
            }
            else
            {
                Velocity *= config.PlayerDrag;
            }
        }

        public void UpdatePosition(float deltaTime, GameConfig config)
        {
            _position += Velocity * deltaTime;
            _position = WrapPosition(_position, config);
        }

        public void UpdateLaser(float deltaTime, GameConfig config)
        {
            if (LaserCharges < config.MaxLaserCharges)
            {
                LaserCooldown += deltaTime;

                if (LaserCooldown >= config.LaserRechargeTime)
                {
                    LaserCharges++;
                    LaserCooldown = 0f;
                    OnLaserChargesChanged?.Invoke(LaserCharges);
                }
            }
        }

        public bool TryFireLaser()
        {
            if (LaserCharges > 0)
            {
                LaserCharges--;
                LaserCooldown = 0f;
                OnLaserChargesChanged?.Invoke(LaserCharges);

                return true;
            }

            return false;
        }

        public void Kill()
        {
            if (IsAlive)
            {
                IsAlive = false;
                OnDestroyed?.Invoke(this);
            }
        }

        private Vector2 WrapPosition(Vector2 position, GameConfig config)
        {
            var halfWidth = config.ScreenWidth / 2f;
            var halfHeight = config.ScreenHeight / 2f;

            if (position.x > halfWidth)
                position.x = -halfWidth;
            else if (position.x < -halfWidth)
                position.x = halfWidth;

            if (position.y > halfHeight)
                position.y = -halfHeight;
            else if (position.y < -halfHeight)
                position.y = halfHeight;

            return position;
        }
    }
}