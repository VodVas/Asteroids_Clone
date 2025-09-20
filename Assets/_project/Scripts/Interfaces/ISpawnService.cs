using UnityEngine;

namespace AsteroidsClone
{
    public interface ISpawnService
    {
        void Update(float deltaTime);
        void SpawnAsteroid(Vector2? position = null, int size = 0);
        void SpawnUfo();
        void SpawnBullet(Vector2 position, Vector2 direction, Vector2 playerVelocity);
        void Reset();
    }
}