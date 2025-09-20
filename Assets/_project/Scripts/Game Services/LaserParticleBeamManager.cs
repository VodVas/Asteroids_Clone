using UnityEngine;

namespace AsteroidsClone
{
    public sealed class LaserParticleBeamManager
    {
        private ParticleSystem _laserParticle;
        private GameConfig _config;
        private float _laserTimer;
        private bool _isLaserActive;

        public void Initialize(ParticleSystem laserParticle, GameConfig config)
        {
            _laserParticle = laserParticle;
            _config = config;
        }

        public void Update(float deltaTime)
        {
            if (_isLaserActive)
            {
                _laserTimer -= deltaTime;

                if (_laserTimer <= 0f)
                {
                    StopLaser();
                }
            }
        }

        public void FireLaser(Vector2 origin, Vector2 direction)
        {
            _laserParticle.transform.position = origin;

            Vector3 direction3D = new Vector3(direction.x, direction.y, 0);
            Vector3 up = Vector3.forward;

            _laserParticle.transform.rotation = Quaternion.LookRotation(direction3D, up);

            _laserParticle.Play();
            _laserTimer = _config.LaserVisualActiveTime;
            _isLaserActive = true;
        }

        private void StopLaser()
        {
            _laserParticle.Stop();
            _isLaserActive = false;
        }
    }
}