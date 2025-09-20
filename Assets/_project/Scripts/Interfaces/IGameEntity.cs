using UnityEngine;

namespace AsteroidsClone
{
    public interface IGameEntity
    {
        int Id { get; }
        Vector2 Position { get; }
        Vector2 Velocity { get; }
        float Rotation { get; }
        bool IsActive { get; }
        EntityType Type { get; }
        void Update(float deltaTime, GameConfig config);
        void Destroy();
    }
}