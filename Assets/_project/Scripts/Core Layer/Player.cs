using System;
using UnityEngine;

namespace AsteroidsClone
{
    public sealed class Player
    {
        public event Action<Player> OnDestroyed;
        public event Action<int> OnLaserChargesChanged;

        private Vector2 _position;
        private Vector2 _velocity;
        private float _rotation;
        private int _laserCharges;
        private float _laserCooldown;
        private bool _isThrusting;
        private bool _isAlive;

        public Vector2 Position => _position;
        public Vector2 Velocity => _velocity;
        public float Rotation => _rotation;
        public int LaserCharges => _laserCharges;
        public float LaserCooldown => _laserCooldown;
        public bool IsThrusting => _isThrusting;
        public bool IsAlive => _isAlive;
        public float Speed => _velocity.magnitude;

        public Player(GameConfig config)
        {
            Reset(config);
        }

        public void Reset(GameConfig config)
        {
            _position = Vector2.zero;
            _velocity = Vector2.zero;
            _rotation = 0f;
            _laserCharges = config.MaxLaserCharges;
            _laserCooldown = 0f;
            _isThrusting = false;
            _isAlive = true;
        }

        public void Rotate(float input, float deltaTime, GameConfig config)
        {
            _rotation += input * config.PlayerRotationSpeed * deltaTime;
            _rotation = (_rotation % 360f + 360f) % 360f;
        }

        public void Thrust(bool isThrusting, float deltaTime, GameConfig config)
        {
            _isThrusting = isThrusting;

            if (isThrusting)
            {
                var radians = _rotation * Mathf.Deg2Rad;
                var direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

                _velocity += direction * config.PlayerAcceleration * deltaTime;

                if (_velocity.magnitude > config.PlayerMaxSpeed)
                {
                    _velocity = _velocity.normalized * config.PlayerMaxSpeed;
                }
            }
            else
            {
                _velocity *= config.PlayerDrag;
            }
        }

        public void UpdatePosition(float deltaTime, GameConfig config)
        {
            _position += _velocity * deltaTime;
            _position = WrapPosition(_position, config);
        }

        public void UpdateLaser(float deltaTime, GameConfig config)
        {
            if (_laserCharges < config.MaxLaserCharges)
            {
                _laserCooldown += deltaTime;

                if (_laserCooldown >= config.LaserRechargeTime)
                {
                    _laserCharges++;
                    _laserCooldown = 0f;
                    OnLaserChargesChanged?.Invoke(_laserCharges);
                }
            }
        }

        public bool TryFireLaser()
        {
            if (_laserCharges > 0)
            {
                _laserCharges--;
                _laserCooldown = 0f;
                OnLaserChargesChanged?.Invoke(_laserCharges);

                return true;
            }

            return false;
        }

        public void Kill()
        {
            if (_isAlive)
            {
                _isAlive = false;
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