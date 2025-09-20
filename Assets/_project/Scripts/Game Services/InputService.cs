using UnityEngine;
using Zenject;

namespace AsteroidsClone
{
    public sealed class InputService : IInputService
    {
        private GameConfig _config;

        public bool IsThrusting => Input.GetKey(_config.ThrustKey);
        public float RotationInput => -Input.GetAxis(_config.RotationAxis);
        public bool FireBullet => Input.GetKeyDown(_config.BulletKey);
        public bool FireLaser => Input.GetKeyDown(_config.LaserKey);
        public bool RestartGame => Input.GetKeyDown(_config.RestartKey);

        [Inject]
        public void Construct(GameConfig config)
        {
            _config = config;
        }
    }
}