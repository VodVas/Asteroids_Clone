using UnityEngine;

namespace AsteroidsClone
{
    public interface ICollisionDetector
    {
        void CheckCollisions();
        void HandleLaserFire(Vector2 origin, Vector2 direction);
    }
}